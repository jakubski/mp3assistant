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
            ApplicationPage page = (ApplicationPage)value;
            ApplicationPageType type = page.Type;
            ViewModel dataContext = page.ViewModel;

            switch (type)
            {
                case ApplicationPageType.Blank:
                    return new Page();
                //case ApplicationPageType.BaseDirectoryViewPage:
                //    return new BaseDirectoryViewPage(dataContext);
                //case ApplicationPageType.DirectoryTree:
                //    return new DirectoryTreePage(dataContext);
                case ApplicationPageType.MainPage:
                    return new MainPage(dataContext);
                case ApplicationPageType.MainPageNavigationBar:
                    return new MainPageNavigationPage(dataContext);
                case ApplicationPageType.FileExplorerPage:
                    return new FileExplorerPage(dataContext);
                case ApplicationPageType.SongEditorPage:
                    return new SongEditorPage(dataContext);
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
