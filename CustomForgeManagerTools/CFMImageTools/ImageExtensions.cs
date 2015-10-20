#define externalapp
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
#if externalapp
            var sourcePath = "";
            var destinationPath = "";
            var xSize = image.Width;
            var ySize = image.Height;
            var cmdArgs = String.Format(" -file \"{0}\" -prescale {2} {3} -quality_highest -max -dxt5 -nomipmap "+
            "-alpha -overwrite -output \"{1}\"", sourcePath, destinationPath, xSize, ySize);

            var rootPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(rootPath, "nvdxt.exe"),
                WorkingDirectory = rootPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = cmdArgs
            };
            Process process = new Process() {StartInfo = startInfo };
            process.Start();
            process.WaitForExit();

            File.Delete(sourcePath);
            if (File.Exists(destinationPath))
            {
                MemoryStream ms = new MemoryStream();
                using (var fs = File.OpenRead(destinationPath))
                {
                    fs.CopyTo(ms);
                }

                File.Delete(destinationPath);
                return ms;
            }
            return null;
#else
            return new MemoryStream(DDSDecompressor.BmpToDDS(image));
#endif
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








        unsafe static void EmitByte(byte b, ref byte* outData)
        {
            outData[0] = b;
            outData += 1;
        }

        unsafe static void EmitWord(int s, ref byte* outData)
        {
            outData[0] = (byte)((s >> 0) & 255);
            outData[1] = (byte)((s >> 8) & 255);
            outData += 2;
        }

        unsafe static void EmitInt32(int i, ref byte* outData)
        {
            outData[0] = (byte)((i >> 0) & 255);
            outData[1] = (byte)((i >> 8) & 255);
            outData[2] = (byte)((i >> 16) & 255);
            outData[3] = (byte)((i >> 24) & 255);
            outData += 4;
        }

        unsafe static void EmitUInt32(uint i, ref byte* outData)
        {
            outData[0] = (byte)((i >> 0) & 255);
            outData[1] = (byte)((i >> 8) & 255);
            outData[2] = (byte)((i >> 16) & 255);
            outData[3] = (byte)((i >> 24) & 255);
            outData += 4;
        }

        //uint


        //unsafe static Point GetMinMaxColorsByBBox(byte* colorBlock)
        //{
        //    int i;
        //    byte[] inset = new byte[3];
        //    byte[] minColor = new byte[4];
        //    byte[] maxColor = new byte[4];

        //    minColor[0] = minColor[1] = minColor[2] = 255;
        //    maxColor[0] = maxColor[1] = maxColor[2] = 0;
        //    for (i = 0; i < 16; i++)
        //    {
        //        if (colorBlock[i * 4 + 0] < minColor[0])
        //            minColor[0] = colorBlock[i * 4 + 0];
        //        if (colorBlock[i * 4 + 1] < minColor[1])
        //            minColor[1] = colorBlock[i * 4 + 1];
        //        if (colorBlock[i * 4 + 2] < minColor[2])
        //            minColor[2] = colorBlock[i * 4 + 2];
        //        if (colorBlock[i * 4 + 0] > maxColor[0])
        //            maxColor[0] = colorBlock[i * 4 + 0];
        //        if (colorBlock[i * 4 + 1] > maxColor[1])
        //            maxColor[1] = colorBlock[i * 4 + 1];
        //        if (colorBlock[i * 4 + 2] > maxColor[2])
        //            maxColor[2] = colorBlock[i * 4 + 2];
        //    }
        //    inset[0] = (byte)((maxColor[0] - minColor[0]) >> DefineConstants.INSET_SHIFT);
        //    inset[1] = (byte)((maxColor[1] - minColor[1]) >> DefineConstants.INSET_SHIFT);
        //    inset[2] = (byte)((maxColor[2] - minColor[2]) >> DefineConstants.INSET_SHIFT);
        //    minColor[0] = (byte)((minColor[0] + inset[0] <= 255) ? minColor[0] + inset[0] : 255);
        //    minColor[1] = (byte)((minColor[1] + inset[1] <= 255) ? minColor[1] + inset[1] : 255);
        //    minColor[2] = (byte)((minColor[2] + inset[2] <= 255) ? minColor[2] + inset[2] : 255);
        //    maxColor[0] = (byte)((maxColor[0] >= inset[0]) ? maxColor[0] - inset[0] : 0);
        //    maxColor[1] = (byte)((maxColor[1] >= inset[1]) ? maxColor[1] - inset[1] : 0);
        //    maxColor[2] = (byte)((maxColor[2] >= inset[2]) ? maxColor[2] - inset[2] : 0);

        //    return new Point(BitConverter.ToInt32(minColor, 0), BitConverter.ToInt32(maxColor, 0));

        //}


        unsafe static void GetMinMaxColorsByBBox(byte* colorBlock, byte* minColor, byte* maxColor)
        {
            int i;
            int[] inset = new int[3];
            minColor[0] = minColor[1] = minColor[2] = 255;
            maxColor[0] = maxColor[1] = maxColor[2] = 0;
            for (i = 0; i < 16; i++)
            {
                if (colorBlock[i * 4 + 0] < minColor[0])
                    minColor[0] = colorBlock[i * 4 + 0];
                if (colorBlock[i * 4 + 1] < minColor[1])
                    minColor[1] = colorBlock[i * 4 + 1];
                if (colorBlock[i * 4 + 2] < minColor[2])
                    minColor[2] = colorBlock[i * 4 + 2];
                if (colorBlock[i * 4 + 0] > maxColor[0])
                    maxColor[0] = colorBlock[i * 4 + 0];
                if (colorBlock[i * 4 + 1] > maxColor[1])
                    maxColor[1] = colorBlock[i * 4 + 1];
                if (colorBlock[i * 4 + 2] > maxColor[2])
                    maxColor[2] = colorBlock[i * 4 + 2];
            }



            inset[0] = (maxColor[0] - minColor[0]) >> DefineConstants.INSET_SHIFT;
            inset[1] = (maxColor[1] - minColor[1]) >> DefineConstants.INSET_SHIFT;
            inset[2] = (maxColor[2] - minColor[2]) >> DefineConstants.INSET_SHIFT;



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

        private static byte GetBitsPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                default:
                    throw new ArgumentException("Only 24 and 32 bit images are supported");

            }
        }

        public unsafe static byte[] BmpToDDS(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            int size = bmpData.Stride * bmpData.Height;
            var outBuff = Marshal.AllocCoTaskMem(size);
            byte* oB = (byte*)outBuff.ToPointer();

            CompressImageDXT1x(bmpData, oB, ref size);
            bmp.UnlockBits(bmpData);

            DDS_HEADER ddh = new DDS_HEADER()
            {
                dwFlags = DDSImage.DDSD_CAPS | DDSImage.DDSD_HEIGHT | DDSImage.DDSD_WIDTH |
                DDSImage.DDSD_PIXELFORMAT | DDSImage.DDSD_LINEARSIZE,
                dwHeight = bmp.Height,
                dwWidth = bmp.Width,
                dwPitchOrLinearSize = size,
                dwCaps = DDSImage.DDSD_PIXELFORMAT,
            };
            ddh.ddspf.dwFlags = DDSImage.DDPF_FOURCC;
            ddh.ddspf.dwFourCC = 827611204;

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                //write header
                ddh.write(bw, true);
                //write the data
                for (int i = 0; i < size; i++)
                    bw.Write(oB[i]);
                ms.Position = 0;
                return ms.ToArray();
            }
        }



        unsafe static void ExtractBlock(byte* inPtr, int width, byte* colorBlock)
        {
            for (int j = 0; j < 4; j++)
            {
                var cbPos = (j * 4 * 4);
                for (int i = 0; i < 16; i += 4)
                {
                    colorBlock[cbPos + i] = inPtr[i];//b
                    colorBlock[cbPos + i + 1] = inPtr[i + 1];//g
                    colorBlock[cbPos + i + 2] = inPtr[i+2 ];//r
                    colorBlock[cbPos + i + 3] = inPtr[i + 3];//a
                }
                inPtr += width *4;
            }
           
        /*  for ( int j = 0; j < 4; j++ ) {
                memcpy( &colorBlock[j*4*4], inPtr, 4*4 );
                inPtr += width * 4;    
            }
            */    
        }
        //574
        #if ddslogger
        static DDSLogger logger = new DDSLogger("out\\DDSWriter.txt");
#endif
        unsafe static void CompressImageDXT1x(BitmapData bmpData, byte* outBuf, ref int outputBytes)
        {
            var inBuf = (byte*)bmpData.Scan0.ToPointer();
            byte* outData = outBuf;
            var blockX = Marshal.AllocCoTaskMem(64);
            var _minColor = Marshal.AllocCoTaskMem(4);
            var _maxColor = Marshal.AllocCoTaskMem(4);

            byte* Block = (byte*)blockX.ToPointer();
            byte* maxColor = (byte*)_maxColor.ToPointer();
            byte* minColor = (byte*)_minColor.ToPointer();

        
            for (int j = 0; j < bmpData.Height; j += 4 )
            {
                for (int i = 0; i < bmpData.Width; i += 4)
                {
                    byte* data = inBuf + j * bmpData.Stride + i * 4;
                    //byte* data = inBuf + i * 4;

                    ExtractBlock(data, bmpData.Width, Block);//, bitsPerPixel == 32);
                    GetMinMaxColorsByBBox(Block, minColor, maxColor);

                    int Max = ColorTo565(maxColor);
                    int Min = ColorTo565(minColor);
                    EmitWord(Max, ref outData);
                    EmitWord(Min, ref outData);
                    EmitColorIndicesFast(Block, maxColor, minColor, ref outData);
                }
            }

            Marshal.FreeCoTaskMem(blockX);
            Marshal.FreeCoTaskMem(_minColor);
            Marshal.FreeCoTaskMem(_maxColor);
            outputBytes = (int)(outData - outBuf);
        }
        static int www = 0;


        unsafe static int ColorFromBytes(byte* color)
        {
            return ((((color[3] << 0x18) | (color[2] << 0x10)) | (color[1] << 8)) | color[0]);
        }


        unsafe static int ColorTo565(byte* color)
        {
            return ((color[2] >> 3) << 11) |
                   ((color[1] >> 2) << 5) |
                   (color[0] >> 3);


            /*            
            ushort maxColor = (ushort)(blockStorage[0] | blockStorage[1] << 8);

            int temp = (maxColor >> 11) * 255 + 16;
            byte r0 = (byte)((temp / 32 + temp) / 32);
             * 
            temp = ((maxColor & 0x07E0) >> 5) * 255 + 32;
            byte g0 = (byte)((temp / 64 + temp) / 64);
             * 
            temp = (maxColor & 0x001F) * 255 + 16;
            byte b0 = (byte)((temp / 32 + temp) / 32);
             * 
             */
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeRush", "Complex member")]
        unsafe static void EmitColorIndicesFast(byte* colorBlock, byte* minColor, byte* maxColor, ref byte* outData)
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

                int b0 = d0 > d3 ? 1 : 0;
                int b1 = d1 > d2 ? 1 : 0;
                int b2 = d0 > d2 ? 1 : 0;
                int b3 = d1 > d3 ? 1 : 0;
                int b4 = d2 > d3 ? 1 : 0;

                int x0 = b1 & b2;
                int x1 = b0 & b3;
                int x2 = b0 & b4;
                result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
            }
            EmitInt32(result, ref outData);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeRush", "Complex member")]
        unsafe static void EmitColorIndicesFast(byte* colorBlock, int MinColor, int MaxColor, ref byte* outData)
        {
            ushort[,] colors = new ushort[4, 4];
            byte[] minColor = BitConverter.GetBytes(MinColor);
            //var m = minColor[1];
            //minColor[1] = minColor[0];
            //minColor[0] = m;

            byte[] maxColor = BitConverter.GetBytes(MaxColor);
            //m = maxColor[1];
            //maxColor[1] = maxColor[0];
            //maxColor[0] = m;

            int result = 0;

            colors[0, 0] = (ushort)((maxColor[0] & DefineConstants.C565_5_MASK) | (maxColor[0] >> 5));
            colors[0, 1] = (ushort)((maxColor[1] & DefineConstants.C565_6_MASK) | (maxColor[1] >> 6));
            colors[0, 2] = (ushort)((maxColor[2] & DefineConstants.C565_5_MASK) | (maxColor[2] >> 5));
            colors[1, 0] = (ushort)((minColor[0] & DefineConstants.C565_5_MASK) | (minColor[0] >> 5));
            colors[1, 1] = (ushort)((minColor[1] & DefineConstants.C565_6_MASK) | (minColor[1] >> 6));
            colors[1, 2] = (ushort)((minColor[2] & DefineConstants.C565_5_MASK) | (minColor[2] >> 5));
            colors[2, 0] = (ushort)((2 * colors[0, 0] + 1 * colors[1, 0]) / 3);
            colors[2, 1] = (ushort)((2 * colors[0, 1] + 1 * colors[1, 1]) / 3);
            colors[2, 2] = (ushort)((2 * colors[0, 2] + 1 * colors[1, 2]) / 3);
            colors[3, 0] = (ushort)((1 * colors[0, 0] + 2 * colors[1, 0]) / 3);
            colors[3, 1] = (ushort)((1 * colors[0, 1] + 2 * colors[1, 1]) / 3);
            colors[3, 2] = (ushort)((1 * colors[0, 2] + 2 * colors[1, 2]) / 3);

            for (int i = 15; i >= 0; i--)
            {
                //int c0 = colorBlock[i * 4 + 0];
                //int c1 = colorBlock[i * 4 + 1];

                //int d0 = Math.Abs(colors[0, 0] - c0) + Math.Abs(colors[0, 1] - c1);
                //int d1 = Math.Abs(colors[1, 0] - c0) + Math.Abs(colors[1, 1] - c1);
                //int d2 = Math.Abs(colors[2, 0] - c0) + Math.Abs(colors[2, 1] - c1);
                //int d3 = Math.Abs(colors[3, 0] - c0) + Math.Abs(colors[3, 1] - c1);

                //int b0 = d0 > d3 ? 1 : 0;
                //int b1 = d1 > d2 ? 1 : 0;
                //int b2 = d0 > d2 ? 1 : 0;
                //int b3 = d1 > d3 ? 1 : 0;
                //int b4 = d2 > d3 ? 1 : 0;

                //int x0 = b1 & b2;
                //int x1 = b0 & b3;
                //int x2 = b0 & b4;

                //result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
                int c0 = colorBlock[i * 4 + 0];
                int c1 = colorBlock[i * 4 + 1];
                int c2 = colorBlock[i * 4 + 2];
                int d0 = Math.Abs(colors[0, 0] - c0) + Math.Abs(colors[0, 1] - c1) + Math.Abs(colors[0, 2] - c2);
                int d1 = Math.Abs(colors[1, 0] - c0) + Math.Abs(colors[1, 1] - c1) + Math.Abs(colors[1, 2] - c2);
                int d2 = Math.Abs(colors[2, 0] - c0) + Math.Abs(colors[2, 1] - c1) + Math.Abs(colors[2, 2] - c2);
                int d3 = Math.Abs(colors[3, 0] - c0) + Math.Abs(colors[3, 1] - c1) + Math.Abs(colors[3, 2] - c2);

                int b0 = d0 > d3 ? 1 : 0;
                int b1 = d1 > d2 ? 1 : 0;
                int b2 = d0 > d2 ? 1 : 0;
                int b3 = d1 > d3 ? 1 : 0;
                int b4 = d2 > d3 ? 1 : 0;

                int x0 = b1 & b2;
                int x1 = b0 & b3;
                int x2 = b0 & b4;
                result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
            }
            EmitInt32(result, ref outData);
        }

    }
    #endregion

}
