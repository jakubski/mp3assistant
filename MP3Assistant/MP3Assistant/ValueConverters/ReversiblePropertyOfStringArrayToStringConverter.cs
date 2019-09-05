using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MP3Assistant
{
    [ValueConversion(typeof(ReversibleProperty<string[]>), typeof(string))]
    public class ReversiblePropertyOfStringArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var array = (value as ReversibleProperty<string[]>).Value;

            return (array.Count() == 1) ? array[0] : string.Join("; ", array);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}