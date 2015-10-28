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

    public static Bitmap ScaleImage(this Image image, int MaxSize)
    {
        int height = image.Height;
        int width = image.Width;
        if (height > width)
        {
            width = Convert.ToInt32(image.Width * ((double)MaxSize / (double)height));
            height = MaxSize;
        }
        else
        {
            height = Convert.ToInt32(image.Height * ( (double)MaxSize / (double)width));
            width = MaxSize;
        }

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

    [DllImport("DF_DDSImage.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern int ConvertToDDS(IntPtr source, out IntPtr dest, int Size);
    [DllImport("DF_DDSImage.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void FreeDDS(IntPtr source);


    public static Stream ToDDS(this Stream source)
    {
        byte[] bytes = new byte[0];
        if (source is MemoryStream)
        {
            bytes = ((MemoryStream)source).ToArray();

        } else
        {
            using (MemoryStream ms = new MemoryStream())
            {
                source.CopyTo(ms);
                bytes = ms.ToArray();
            }
        }
        IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
        try
        {
            Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
            var dest = IntPtr.Zero;
            var result = ConvertToDDS(unmanagedPointer, out dest, bytes.Length);
            if (result > 0)
            {
                byte[] arr = new byte[result];
                Marshal.Copy(dest, arr, 0, result);
                FreeDDS(dest);
                return new MemoryStream(arr);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(unmanagedPointer);
        }
        return new MemoryStream();   
    }


    public static Stream ToDDS(this Stream source, int width, int height)
    {
        using (Image i = Image.FromStream(source))
            return i.ToDDS(width, height);
    }


    public static Stream ToDDS(this Image image)
    {    
        using (MemoryStream source = new MemoryStream())
        {
            image.Save(source, ImageFormat.Png);
            var bytes = source.ToArray();

            IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
                var dest = IntPtr.Zero;
                var result = ConvertToDDS(unmanagedPointer, out dest, bytes.Length);
                if (result > 0)
                {
                    byte[] arr = new byte[result];
                    Marshal.Copy(dest, arr, 0, result);
                    FreeDDS(dest);
                    return new MemoryStream(arr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedPointer);
            }
        }
        return new MemoryStream();           
    }

    public static Stream ToDDS(this Image image, int width, int height)
    {
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