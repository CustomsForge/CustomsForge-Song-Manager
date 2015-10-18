using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CustomsForgeManagerTools
{
    public static class ImageExtensions
    {
        //Credits to Mark from StackOverflow
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        public static Bitmap DDStoBitmap(Stream ddsStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var dds = new DDSImage(ddsStream);
                if (dds.images.Count() > 0)
                    return dds.images[0];
                return null;
            }
        }

        public static Stream ToDDS(this Bitmap image)
        {
            return new MemoryStream(DDSDecompressor.BmpToDDS(image));

            //MemoryStream ms = new MemoryStream();
            //using (MemoryStream imgSource = new MemoryStream())
            //{
            //    image.Save(imgSource, ImageFormat.Bmp);
            //    imgSource.Position = 0;
            //    DevILLite.ConvertImageType(imgSource, DevILLite.ImageType.Dds, ms);
            //    ms.Position = 0;
            //    if (ms.Length == 0)
            //    {
            //        ms.Dispose();
            //        return null;
            //    }
            //}

        }

        public static Stream ToDDS(this Bitmap image, int width, int height)
        {

            //MemoryStream ms = new MemoryStream();
            bool freeImage = false;

            if (image.Width != width || image.Height != height)
            {
                image = image.ResizeImage(width, height);
                freeImage = true;
            }
            var x = ToDDS(image);

            if (freeImage)
                image.Dispose();
            return x;

        }


    }

    #region ddscompressor
    class DDSDecompressor
    {
        internal static partial class DefineConstants
        {
            public const int C565_5_MASK = 0xF8; // 0xFF minus last three bits
            public const int C565_6_MASK = 0xFC; // 0xFF minus last two bits
            public const int INSET_SHIFT = 4; // inset the bounding box with ( range >> shift )
            public const int MAX_INT = 2147483647; // max value for an int 32
            public const int MIN_INT = -2147483647 - 1; // min value for an int 32
        }

        void RGBAtoYCoCg(byte[] inBuf, byte[] outBuf, int width, int height)
        {
            for (int j = 0; j < width * height; j++)
            {
                byte R = inBuf[j * 4 + 0];
                byte G = inBuf[j * 4 + 1];
                byte B = inBuf[j * 4 + 2];
                byte A = inBuf[j * 4 + 3];
                int Co = R - B;
                int t = B + (Co / 2);
                int Cg = G - t;
                int Y = t + (Cg / 2);

                Co += 128;
                Cg += 96;

                if (Co < 0) Co = 0;
                if (Co > 255) Co = 255;
                if (Cg < 0) Cg = 0;
                if (Cg > 255) Cg = 255;

                outBuf[j * 4 + 0] = (byte)Co;
                outBuf[j * 4 + 1] = (byte)Cg;
                outBuf[j * 4 + 2] = 0;
                outBuf[j * 4 + 3] = (byte)Y;
            }
        }


        static IntPtr getIntPtr(byte[] buffer, int offset)
        {
            IntPtr intPtr;
            unsafe
            {
                fixed (byte* p1 = buffer)
                {
                    byte* p2 = p1 + offset;
                    intPtr = (IntPtr)p2;
                }
            }
            return intPtr;
        }

        static void memcpy(byte[] dst, int dstOffset, byte[] src, int srcOffset, int len)
        {
            IntPtr intPtr = getIntPtr(dst, dstOffset);
            Marshal.Copy(src, srcOffset, intPtr, len);
        }

        unsafe static void ExtractBlock(byte* inPtr, int width, byte* colorBlock)
        {
            var pos = 0;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 16; i++)
                {
                    colorBlock[j * 4 * 4 + i] = inPtr[pos + i];
                }
            //    var bdest = j * 16;

            //  //  var cP = (IntPtr)colorBlock[j * 16];

            //    int size = 4; // first byte is size;
            //    //byte[] target = new byte[size];
            //    for (int i = 0; i < size; ++i)
            //        colorBlock[bdest + i] = inPtr[pos + i];
            //    //    target[i] = inPtr[i + 1 + pos];



            //    //Marshal.Copy(iP, 0, colorBlock[bdest], size);
                pos += width * 4;
            //    /* memcpy( &colorBlock[j*4*4], inPtr, 4*4 );
            //    inPtr += width * 4;    */
            }
        }


        unsafe static int ColorTo565(byte* color)
        {
            return ((color[0] >> 3) << 11) |
                   ((color[1] >> 2) << 5) |
                   (color[2] >> 3);
        }

        unsafe static void EmitByte(byte b, byte* outData)
        {
            outData[0] = b;
            outData += 1;
        }

        unsafe static void EmitWord(short s, byte* outData)
        {
            outData[0] = (byte)((s >> 0) & 255);
            outData[1] = (byte)((s >> 8) & 255);
            outData += 2;
        }

        unsafe static void EmitDoubleWord(int i, byte* outData)
        {
            outData[0] = (byte)((i >> 0) & 255);
            outData[1] = (byte)((i >> 8) & 255);
            outData[2] = (byte)((i >> 16) & 255);
            outData[3] = (byte)((i >> 24) & 255);
            outData += 4;
        }

        unsafe static void EmitColorIndices(byte* colorBlock, byte[] minColor, byte[] maxColor, byte* outData)
        {
            byte[,] colors = new byte[4, 4];
            int[] indices = new int[16];
            colors[0, 0] = (byte)((maxColor[0] & DefineConstants.C565_5_MASK) | (maxColor[0] >> 5));
            colors[0, 1] = (byte)((maxColor[1] & DefineConstants.C565_6_MASK) | (maxColor[1] >> 6));
            colors[0, 2] = (byte)((maxColor[2] & DefineConstants.C565_5_MASK) | (maxColor[2] >> 5));
            colors[1, 0] = (byte)((minColor[0] & DefineConstants.C565_5_MASK) | (minColor[0] >> 5));
            colors[1, 1] = (byte)((minColor[1] & DefineConstants.C565_6_MASK) | (minColor[1] >> 6));
            colors[1, 2] = (byte)((minColor[2] & DefineConstants.C565_5_MASK) | (minColor[2] >> 5));
            colors[2, 0] = (byte)((2 * colors[0, 0] + 1 * colors[1, 0]) / 3);
            colors[2, 1] = (byte)((2 * colors[0, 1] + 1 * colors[1, 1]) / 3);
            colors[2, 2] = (byte)((2 * colors[0, 2] + 1 * colors[1, 2]) / 3);
            colors[3, 0] = (byte)((1 * colors[0, 0] + 2 * colors[1, 0]) / 3);
            colors[3, 1] = (byte)((1 * colors[0, 1] + 2 * colors[1, 1]) / 3);
            colors[3, 2] = (byte)((1 * colors[0, 2] + 2 * colors[1, 2]) / 3);
            for (int i = 0; i < 16; i++)
            {
                int minDistance = DefineConstants.MAX_INT;
                for (int j = 0; j < 4; j++)
                {
                    var x = i * 4;

                    int dist =
                        ColorDistance(
                            new byte[] { colorBlock[x], colorBlock[x + 1], colorBlock[x + 2] },
                            new byte[] { colors[j, 0], colors[j, 1], colors[j, 2] }
                        );
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        indices[i] = j;
                    }
                }
            }
            int result = 0;
            for (int i = 0; i < 16; i++)
            {
                result |= (indices[i] << (i << 1));
            }
            EmitDoubleWord(result, outData);
        }

        private static int ColorLuminance(byte[] color)
        {
            return (color[0] + color[1] * 2 + color[2]);
        }

        unsafe static int ColorDistance(byte[] c1, byte[] c2)
        {
            return ((c1[0] - c2[0]) * (c1[0] - c2[0])) +
              ((c1[1] - c2[1]) * (c1[1] - c2[1])) +
              ((c1[2] - c2[2]) * (c1[2] - c2[2]));
        }

        unsafe static void SwapColors(byte[] c1, byte[] c2)
        {
            byte[] tm = new byte[3];
            Array.Copy(c1, tm, 3);
            Array.Copy(c2, c1, 3);
            Array.Copy(tm, c2, 3);
        }

        unsafe static void GetMinMaxColorsByBBox(byte* colorBlock, byte* minColor, byte* maxColor)
        {
            int i;
            byte[] inset = new byte[3];
            minColor[0] = minColor[1] = minColor[2] = 255;
            maxColor[0] = maxColor[1] = maxColor[2] = 0;
            for (i = 0; i < 16; i++)
            {
                if (colorBlock[i * 4 + 0] < minColor[0]) { minColor[0] = colorBlock[i * 4 + 0]; }
                if (colorBlock[i * 4 + 1] < minColor[1]) { minColor[1] = colorBlock[i * 4 + 1]; }
                if (colorBlock[i * 4 + 2] < minColor[2]) { minColor[2] = colorBlock[i * 4 + 2]; }
                if (colorBlock[i * 4 + 0] > maxColor[0]) { maxColor[0] = colorBlock[i * 4 + 0]; }
                if (colorBlock[i * 4 + 1] > maxColor[1]) { maxColor[1] = colorBlock[i * 4 + 1]; }
                if (colorBlock[i * 4 + 2] > maxColor[2]) { maxColor[2] = colorBlock[i * 4 + 2]; }
            }
            inset[0] = (byte)((maxColor[0] - minColor[0]) >> DefineConstants.INSET_SHIFT);
            inset[1] = (byte)((maxColor[1] - minColor[1]) >> DefineConstants.INSET_SHIFT);
            inset[2] = (byte)((maxColor[2] - minColor[2]) >> DefineConstants.INSET_SHIFT);
            minColor[0] = (byte)((minColor[0] + inset[0] <= 255) ? minColor[0] + inset[0] : 255);
            minColor[1] = (byte)((minColor[1] + inset[1] <= 255) ? minColor[1] + inset[1] : 255);
            minColor[2] = (byte)((minColor[2] + inset[2] <= 255) ? minColor[2] + inset[2] : 255);
            maxColor[0] = (byte)((maxColor[0] >= inset[0]) ? maxColor[0] - inset[0] : 0);
            maxColor[1] = (byte)((maxColor[1] >= inset[1]) ? maxColor[1] - inset[1] : 0);
            maxColor[2] = (byte)((maxColor[2] >= inset[2]) ? maxColor[2] - inset[2] : 0);
        }

        //void CompressImageDXT5YCoCg(byte[] inBuf, byte[] outBuf, int width, int height,
        //                     ref int outputBytes)
        //{
        //    byte[] tmpBuf = new byte[width * height * 4];
        //    RGBAtoYCoCg(inBuf, tmpBuf, width, height);
        //    CompressImageDXT5(tmpBuf, outBuf, width, height, ref outputBytes);

        //}

        //void CompressImageDXT5(byte[] inBuf, byte[] outBuf, int width, int height, ref int outputBytes)
        //{


        //}

        public static byte[] BmpToDDS(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,  bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int size = GetImgLength(bmp.Width, bmp.Height);

            var outBuff = Marshal.AllocCoTaskMem(size);
            unsafe
            {
                byte* oB = (byte*)outBuff;
                byte* iB = (byte*)ptr;
                CompressImageDXT1(iB, oB, bmp.Width, bmp.Height, ref size);

            }
            //byte[] outBuff = new byte[size];


            bmp.UnlockBits(bmpData);

            return new byte[] { };

        }

        static int GetImgLength(int width, int height)
        {
            return (width * height) * 64;//64 to be safe?
        }

        unsafe static void CompressImageDXT1(byte* inBuf, byte* outBuf, int width, int height, ref int outputBytes)
        {

            byte* outData = outBuf;
            var blockX = Marshal.AllocCoTaskMem(64);
            var minColor = Marshal.AllocCoTaskMem(4);
            var maxColor = Marshal.AllocCoTaskMem(4);

            byte* Block = (byte*)blockX.ToPointer();
          //  byte[] Block = new byte[64];
            byte* MaxColor = (byte*)maxColor.ToPointer();
            byte* MinColor = (byte*)minColor.ToPointer();
            //   outData = outBuf;
            for (int j = 0; j < height; j += 4, inBuf += width * 4 * 4)
            {
                for (int i = 0; i < width; i += 4)
                {
                    ExtractBlock(inBuf + i * 4, width, Block);
                    GetMinMaxColorsByBBox(Block, MinColor, MaxColor);

                    EmitWord((short)ColorTo565(MaxColor), outData);
                    EmitWord((short)ColorTo565(MinColor), outData);
                    EmitColorIndicesFast(Block, MinColor, MaxColor, outData);
                }
            }
            //Marshal.FreeCoTaskMem(blockX);
            Marshal.FreeCoTaskMem(minColor);
            Marshal.FreeCoTaskMem(maxColor);
            outputBytes = (int)(outData - outBuf);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeRush", "Complex member")]
        private void GetMinMaxColorsAlpha(byte[] colorBlock, byte[] minColor, byte[] maxColor)
        {
            int i;
            byte[] inset = new byte[4];
            byte y, cg, co, r, g, b;
            minColor[0] = minColor[1] = minColor[2] = minColor[3] = 255;
            maxColor[0] = maxColor[1] = maxColor[2] = maxColor[3] = 0;
            for (i = 0; i < 16; i++)
            {
                r = colorBlock[i * 4 + 0];
                g = colorBlock[i * 4 + 1];
                b = colorBlock[i * 4 + 2];
                y = (byte)((g >> 1) + ((r + b) >> 2));
                cg = (byte)(g - ((r + b) >> 1));
                co = (byte)(r - b);
                colorBlock[i * 4 + 0] = co;
                colorBlock[i * 4 + 1] = cg;
                colorBlock[i * 4 + 2] = 0;
                colorBlock[i * 4 + 3] = y;
                if (colorBlock[i * 4 + 0] < minColor[0]) { minColor[0] = colorBlock[i * 4 + 0]; }
                if (colorBlock[i * 4 + 1] < minColor[1]) { minColor[1] = colorBlock[i * 4 + 1]; }
                if (colorBlock[i * 4 + 2] < minColor[2]) { minColor[2] = colorBlock[i * 4 + 2]; }
                if (colorBlock[i * 4 + 3] < minColor[3]) { minColor[3] = colorBlock[i * 4 + 3]; }
                if (colorBlock[i * 4 + 0] > maxColor[0]) { maxColor[0] = colorBlock[i * 4 + 0]; }
                if (colorBlock[i * 4 + 1] > maxColor[1]) { maxColor[1] = colorBlock[i * 4 + 1]; }
                if (colorBlock[i * 4 + 2] > maxColor[2]) { maxColor[2] = colorBlock[i * 4 + 2]; }
                if (colorBlock[i * 4 + 3] > maxColor[3]) { maxColor[3] = colorBlock[i * 4 + 3]; }
            }
            inset[0] = (byte)((maxColor[0] - minColor[0]) >> DefineConstants.INSET_SHIFT);
            inset[1] = (byte)((maxColor[1] - minColor[1]) >> DefineConstants.INSET_SHIFT);
            inset[2] = (byte)((maxColor[2] - minColor[2]) >> DefineConstants.INSET_SHIFT);
            inset[3] = (byte)((maxColor[3] - minColor[3]) >> DefineConstants.INSET_SHIFT);
            minColor[0] = (byte)((minColor[0] + inset[0] <= 255) ? minColor[0] + inset[0] : 255);
            minColor[1] = (byte)((minColor[1] + inset[1] <= 255) ? minColor[1] + inset[1] : 255);
            minColor[2] = (byte)((minColor[2] + inset[2] <= 255) ? minColor[2] + inset[2] : 255);
            minColor[3] = (byte)((minColor[3] + inset[3] <= 255) ? minColor[3] + inset[3] : 255);
            maxColor[0] = (byte)((maxColor[0] >= inset[0]) ? maxColor[0] - inset[0] : 0);
            maxColor[1] = (byte)((maxColor[1] >= inset[1]) ? maxColor[1] - inset[1] : 0);
            maxColor[2] = (byte)((maxColor[2] >= inset[2]) ? maxColor[2] - inset[2] : 0);
            maxColor[3] = (byte)((maxColor[3] >= inset[3]) ? maxColor[3] - inset[3] : 0);
        }

        unsafe static void EmitColorIndicesFast(byte* colorBlock, byte* minColor, byte* maxColor, byte* outData)
        {
            ushort[,] colors = new ushort[4, 4];
            int result = 0;

            colors[0,0] = (ushort)((maxColor[0] & DefineConstants.C565_5_MASK) | (maxColor[0] >> 5));
            colors[0,1] = (ushort)((maxColor[1] & DefineConstants.C565_6_MASK) | (maxColor[1] >> 6));
            colors[0,2] = (ushort)((maxColor[2] & DefineConstants.C565_5_MASK) | (maxColor[2] >> 5));
            colors[1,0] = (ushort)((minColor[0] & DefineConstants.C565_5_MASK) | (minColor[0] >> 5));
            colors[1,1] = (ushort)((minColor[1] & DefineConstants.C565_6_MASK) | (minColor[1] >> 6));
            colors[1,2] = (ushort)((minColor[2] & DefineConstants.C565_5_MASK) | (minColor[2] >> 5));
            colors[2,0] = (ushort)((2 * colors[0,0] + 1 * colors[1,0]) / 3);
            colors[2,1] = (ushort)((2 * colors[0,1] + 1 * colors[1,1]) / 3);
            colors[2,2] = (ushort)((2 * colors[0,2] + 1 * colors[1,2]) / 3);
            colors[3,0] = (ushort)((1 * colors[0,0] + 2 * colors[1,0]) / 3);
            colors[3,1] = (ushort)((1 * colors[0,1] + 2 * colors[1,1]) / 3);
            colors[3,2] = (ushort)((1 * colors[0,2] + 2 * colors[1,2]) / 3);

            for (int i = 15; i >= 0; i--)
            {
                int c0 = colorBlock[i * 4 + 0];
                int c1 = colorBlock[i * 4 + 1];
                int c2 = colorBlock[i * 4 + 2];
                int d0 = Math.Abs(colors[0, 0] - c0) + Math.Abs(colors[0, 1] - c1) + Math.Abs(colors[0, 2] - c2);
                int d1 = Math.Abs(colors[1, 0] - c0) + Math.Abs(colors[1, 1] - c1) + Math.Abs(colors[1, 2] - c2);
                int d2 = Math.Abs(colors[2, 0] - c0) + Math.Abs(colors[2, 1] - c1) + Math.Abs(colors[2, 2] - c2);
                int d3 = Math.Abs(colors[3, 0] - c0) + Math.Abs(colors[3, 1] - c1) + Math.Abs(colors[3, 2] - c2);

                //TODO : might be 1 : 0
                int b0 = d0 > d3 ? 0 : 1;
                int b1 = d1 > d2 ? 0 : 1;
                int b2 = d0 > d2 ? 0 : 1;
                int b3 = d1 > d3 ? 0 : 1;
                int b4 = d2 > d3 ? 0 : 1;

                int x0 = b1 & b2;
                int x1 = b0 & b3;
                int x2 = b0 & b4;
                result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
            }
            EmitDoubleWord(result, outData);
        }

    }






    #endregion

}
