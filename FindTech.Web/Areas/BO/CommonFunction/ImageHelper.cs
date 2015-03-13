using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace TestImageCrop
{
    public static class ImageHelper
    {
        public static byte[] CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, width, height);
            }
        }

        public static byte[] CropImage(Stream content, int x, int y, int width, int height)
        {
            //Parsing stream to bitmap
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                //Get new dimensions
                double sourceWidth = Convert.ToDouble(sourceBitmap.Size.Width);
                double sourceHeight = Convert.ToDouble(sourceBitmap.Size.Height);
                Rectangle cropRect = new Rectangle(x, y, width, height);

                //Creating new bitmap with valid dimensions
                using (Bitmap newBitMap = new Bitmap(cropRect.Width, cropRect.Height))
                {
                    using (Graphics g = Graphics.FromImage(newBitMap))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(sourceBitmap, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);

                        return GetBitmapBytes(newBitMap);
                    }
                }
            }
        }

        public static byte[] GetBitmapBytes(Bitmap source)
        {
            //Settings to increase quality of the image
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[4];
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

            //Temporary stream to save the bitmap
            using (MemoryStream tmpStream = new MemoryStream())
            {
                source.Save(tmpStream, codec, parameters);

                //Get image bytes from temporary stream
                byte[] result = new byte[tmpStream.Length];
                tmpStream.Seek(0, SeekOrigin.Begin);
                tmpStream.Read(result, 0, (int)tmpStream.Length);

                return result;
            }
        }

        public static byte[] getResizedImage(String path, int width, int height)
    {
        Bitmap imgIn = new Bitmap(path);
        double y = imgIn.Height;
        double x = imgIn.Width;
                    
        double factor = 1;
        if (width > 0)
        {
            factor = width / x;
        }
        else if (height > 0)
        {
            factor = height / y;
        }
        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
        Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
        
        // Set DPI of image (xDpi, yDpi)
        imgOut.SetResolution(72,72);  
         
        Graphics g = Graphics.FromImage(imgOut);
        g.Clear(Color.White);
        g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), 
          new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);

        imgOut.Save(outStream, getImageFormat(path));
        return outStream.ToArray();
    }

    public static string getContentType(String path)
    {
        switch (Path.GetExtension(path))
        {
            case ".bmp": return "Image/bmp";
            case ".gif": return "Image/gif";
            case ".jpg": return "Image/jpeg";
            case ".png": return "Image/png";
            default: break;
        }
        return "";
    }

    public static ImageFormat getImageFormat(String path)
    {
        switch (Path.GetExtension(path))
        {
            case ".bmp": return ImageFormat.Bmp;
            case ".gif": return ImageFormat.Gif;
            case ".jpg": return ImageFormat.Jpeg;
            case ".png": return ImageFormat.Png;
            default: break;
        }
        return ImageFormat.Jpeg;
    }
    }
}