using System;
using System.IO;
using zlib;


namespace CustomsForgeManager_Winforms.lib
{
    internal static class ZlibNet
    {
        public static void CompressData(byte[] inData, out byte[] outData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream, zlibConst.Z_DEFAULT_COMPRESSION))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                outData = outMemoryStream.ToArray();
            }
        }

        public static void DecompressData(byte[] inData, out byte[] outData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                outData = outMemoryStream.ToArray();
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static void ZipFile(string inFile, string outFile)
        {
            using (FileStream outFileStream = new FileStream(outFile, FileMode.Create))
            using (ZOutputStream outZStream = new ZOutputStream(outFileStream, zlibConst.Z_DEFAULT_COMPRESSION))
            using (FileStream inFileStream = new FileStream(inFile, FileMode.Open))
                try
                {
                    CopyStream(inFileStream, outZStream);
                }
                finally
                {
                    outZStream.Close();
                    outFileStream.Close();
                    inFileStream.Close();
                }
        }

        public static void UnzipFile(string inFile, string outFile)
        {
            using (FileStream outFileStream = new FileStream(outFile, FileMode.Create))
            using (ZOutputStream outZStream = new zlib.ZOutputStream(outFileStream))
            using (FileStream inFileStream = new FileStream(inFile, FileMode.Open))
                try
                {
                    CopyStream(inFileStream, outZStream);
                }
                finally
                {
                    outZStream.Close();
                    outFileStream.Close();
                    inFileStream.Close();
                }
        }

        // alt method
        public static void UncompressFile(string inFile, string outFile)
        {
            int data = 0;
            int stopByte = -1;
            FileStream outFileStream = new FileStream(outFile, FileMode.Create);
            ZInputStream inZStream = new ZInputStream(File.Open(inFile, FileMode.Open, FileAccess.Read));
            while (stopByte != (data = inZStream.Read()))
            {
                byte _dataByte = (byte)data;
                outFileStream.WriteByte(_dataByte);
            }

            inZStream.Close();
            outFileStream.Close();
        }


    }
}

