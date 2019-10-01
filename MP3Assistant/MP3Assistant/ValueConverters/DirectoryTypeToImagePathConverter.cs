using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MP3Assistant
{
    public class DirectoryTypeToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var itemType = (DirectoryType)value;
            string basePath = "pack://application:,,,/Views/Images/";
            string imagePath = "";

            switch (itemType)
            {
                case DirectoryType.Drive:
                    imagePath = "drive.png";
                    break;
                case DirectoryType.Folder:
                    imagePath = "folder.png";
                    break;
                case DirectoryType.File:
                    imagePath = "file.png";
                    break;
                case DirectoryType.MP3File:
                    imagePath = "mp3.png";
                    break;
            }

            return basePath + imagePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
