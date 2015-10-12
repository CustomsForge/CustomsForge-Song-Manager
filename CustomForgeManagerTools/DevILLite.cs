using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CustomsForgeManagerTools
{
    /// <summary>
    /// DevILLite is a static class that uses the DevIL library to perform imaging routines.
    /// </summary>
    public static class DevILLite
    {
        [DllImport("DevIL.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern uint ilGenImage();

        [DllImport("DevIL.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void ilBindImage(uint Image);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("DevIL.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool ilLoadL(uint Type, IntPtr Lump, uint Size);

        [DllImport("DevIL.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern uint ilDetermineTypeL(IntPtr Lump, uint Size);

        [DllImport("DevIL.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern uint ilGetError();


        public enum ImageType
        {
            Blp = 0x44c,
            Bmp = 0x420,
            CHeader = 0x42f,
            Cut = 0x421,
            Dcx = 0x438,
            Dds = 0x437,
            Dicom = 0x44a,
            Doom = 0x422,
            DoomFlat = 0x423,
            Dpx = 0x450,
            Exif = 0x43a,
            Exr = 0x442,
            Fits = 0x449,
            Ftx = 0x44d,
            Gif = 0x436,
            Hdr = 0x43f,
            Icns = 0x440,
            Ico = 0x424,
            Iff = 0x447,
            Ilbm = 0x426,
            Iwi = 0x44b,
            Jfif = 0x425,
            Jng = 0x435,
            Jp2 = 0x441,
            Jpg = 0x425,
            Lif = 0x434,
            Mdl = 0x431,
            Mng = 0x435,
            Mp3 = 0x452,
            Pcd = 0x427,
            Pcx = 0x428,
            Pic = 0x429,
            Pix = 0x43c,
            Png = 0x42a,
            Pnm = 0x42b,
            PSD = 0x439,
            Psp = 0x43b,
            Pxr = 0x43d,
            Raw = 0x430,
            Rot = 0x44e,
            Sgi = 0x42c,
            Sun = 0x446,
            Texture = 0x44f,
            Tga = 0x42d,
            Tiff = 0x42e,
            Tpl = 0x448,
            Unknown = 0,
            Utx = 0x451,
            Vtf = 0x444,
            Wal = 0x432,
            Wbmp = 0x445,
            Wdp = 0x443,
            Xpm = 0x43e
        }

        public enum ErrorType
        {
            BadDimensions = 0x511,
            CouldNotOpenFile = 0x50a,
            Exr_Error = 0x5e7,
            FileAlreadyExists = 0x50c,
            FileReadError = 0x512,
            FileWriteError = 0x512,
            FormatNotSupported = 0x503,
            Gif_Error = 0x5e1,
            IllegalFileHeader = 0x508,
            IllegalFileValue = 0x507,
            IllegalOperation = 0x506,
            InternalError = 0x504,
            InvalidConversion = 0x510,
            InvalidEnum = 0x501,
            InvalidExtension = 0x50b,
            InvalidParameter = 0x509,
            InvalidValue = 0x505,
            Jp2_Error = 0x5e6,
            Jpeg_Error = 0x5e2,
            Mng_Error = 0x5e5,
            NoError = 0,
            OutFormatSame = 0x50d,
            OutOfMemory = 0x502,
            Png_Error = 0x5e3,
            StackOverflow = 0x50e,
            StackUnderflow = 0x50f,
            Tiff_Error = 0x5e4,
            UnknownError = 0x5ff
        }


        /// <summary>
        ///  Gets the last error.
        /// </summary>
        /// <returns>ErrorType</returns>
        public static ErrorType GetError()
        {
            return (ErrorType)ilGetError();
        }

        private static uint GenerateImage()
        {
            return ilGenImage();
        }

        /// <summary>
        /// Determines what type of image format from the parameter.
        /// </summary>
        /// <param name="lump">The data.</param>
        /// <returns>ImageType</returns>
        public static unsafe ImageType DetermineImageType(byte[] lump)
        {
            if ((lump == null) || (lump.Length == 0))
            {
                return ImageType.Unknown;
            }
            uint length = (uint)lump.Length;
            fixed (byte* numRef = lump)
            {
                return (ImageType)ilDetermineTypeL(new IntPtr((void*)numRef), length);
            }
        }

        /// <summary>
        /// Determines what type of image format from the parameter.
        /// </summary>
        /// <param name="stream">The data.</param>
        /// <returns>ImageType</returns>
        public static ImageType DetermineImageType(Stream stream)
        {
            return DetermineImageType(ReadStreamFully(stream, 0));
        }

        private static byte[] ReadStreamFully(Stream stream, int initialLength)
        {
            int numbRead;
            if (initialLength < 1)
            {
                initialLength = 0x8000;
            }
            byte[] buffer = new byte[initialLength];
            int offset = 0;
            while ((numbRead = stream.Read(buffer, offset, buffer.Length - offset)) > 0)
            {
                offset += numbRead;
                if (offset == buffer.Length)
                {
                    int num3 = stream.ReadByte();
                    if (num3 == -1)
                    {
                        return buffer;
                    }
                    byte[] buffer2 = new byte[buffer.Length * 2];
                    Array.Copy(buffer, buffer2, buffer.Length);
                    buffer2[offset] = (byte)num3;
                    buffer = buffer2;
                    offset++;
                }
            }
            byte[] destinationArray = new byte[offset];
            Array.Copy(buffer, destinationArray, offset);
            return destinationArray;
        }

        private static unsafe bool LoadImageFromStream(Stream stream)
        {
            if (!((stream != null) && stream.CanRead))
            {
                return false;
            }
            byte[] lump = ReadStreamFully(stream, 0);
            uint length = (uint)lump.Length;
            bool flag = false;
            ImageType type = DetermineImageType(lump);
            fixed (byte* numRef = lump)
            {
                flag = ilLoadL((uint)type, new IntPtr((void*)numRef), length);
            }
            return flag;
        }

        private static uint GenImage()
        {
            var imageID = GenerateImage();
            ilBindImage(imageID);
            return imageID;
        }

        private static uint LoadFromStream(Stream stream)
        {
            if (!((stream != null) && stream.CanRead))
            {
                throw new IOException("Failed to load image, Stream is null or write-only");
            }
            var id = GenImage();
            if (!LoadImageFromStream(stream))
            {
                throw new IOException(string.Format("Failed to loade image: {0}", GetError()));
            }
            return id;
        }

        //private static bool SaveToStream(uint image, ImageType imageType, Stream stream)
        //{
        //    if (!(((image >= 0 && (imageType != ImageType.Unknown)) &&
        //        (stream != null)) && stream.CanWrite))
        //    {
        //        return false;
        //    }
        //    ilBindImage(image);
        //    return SaveImageToStream(imageType, stream);
        //}

        /// <summary>
        /// Convert an image from one type to another.
        /// </summary>
        /// <param name="Source">The source stream.</param>
        /// <param name="imageType">The destination image type.</param>
        /// <param name="Destination">The destination stream.</param>
        public static bool ConvertImageType(Stream Source, ImageType imageType, Stream Destination)
        {
            if (!(((Source != null && (imageType != ImageType.Unknown)) &&
                (Destination != null)) && Destination.CanWrite) && Source.CanRead)
            {
                return false;
            }
            var x = LoadFromStream(Source);
            ilBindImage(x);
            return SaveImageToStream(imageType, Destination);
        }



        [DllImport("DevIL.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern uint ilSaveL(uint Type, IntPtr Lump, uint Size);

        private static unsafe bool SaveImageToStream(ImageType imageType, Stream stream)
        {
            if (!(((imageType != ImageType.Unknown) && (stream != null)) && stream.CanWrite))
            {
                return false;
            }
            uint size = ilSaveL((uint)imageType, IntPtr.Zero, 0);
            if (size == 0)
            {
                return false;
            }
            byte[] buffer = new byte[size];
            fixed (byte* numRef = buffer)
            {
                if (ilSaveL((uint)imageType, new IntPtr((void*)numRef), size) == 0)
                {
                    return false;
                }
            }
            stream.Write(buffer, 0, buffer.Length);
            return true;
        }
    }
}
