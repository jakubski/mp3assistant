using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MP3Assistant
{
    public static class ImageHelpers
    {
        public static byte[] FileToBytes(string path)
        {
            Image image = Image.FromFile(path);
            byte[] array;

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, GetFormat(path));
                array = stream.ToArray();
            }

            return array;
        }

        private static ImageFormat GetFormat(string path)
        {
            switch (DirectoryHelpers.GetExtension(path))
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Bmp;
            }
        }
    }
}
