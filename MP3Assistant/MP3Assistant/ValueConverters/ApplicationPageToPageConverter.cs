using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace MP3Assistant
{
    public class ApplicationPageToPageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Blank:
                    return new Page();
                case ApplicationPage.BaseDirectoryViewPage:
                    return new BaseDirectoryViewPage();
                case ApplicationPage.DirectoryTree:
                    return new DirectoryTreePage();
                case ApplicationPage.MainPage:
                    return new MainPage();
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
