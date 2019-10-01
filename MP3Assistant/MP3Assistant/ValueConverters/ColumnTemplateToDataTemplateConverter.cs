using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MP3Assistant
{
    class ColumnTemplateToDataTemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (ColumnTemplate)value;
            string resource = "";

            switch (type)
            {
                case ColumnTemplate.Filename:
                    resource = "FilenameCellTemplate";
                    break;
            }

            return Application.Current.TryFindResource(resource);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
