using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MP3Assistant
{
    public class MultiValueBottomTextBlockTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var areAttributeValuesDifferent = (bool)values[0];
            var topTextBoxText = (string)values[1];
            var aggregatedValue = values[2];

            if (areAttributeValuesDifferent && topTextBoxText.Length == 0)
                return aggregatedValue;
            else
                return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
