using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MP3Assistant
{
    public class MultiValueTopTextBoxTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var areAttributeValuesDifferent = (bool)values[0];
            var value = values[1];

            if (areAttributeValuesDifferent)
                return "";
            else
                return value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing, value };
        }
    }
}
