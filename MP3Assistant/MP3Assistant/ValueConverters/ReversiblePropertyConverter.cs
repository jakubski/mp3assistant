using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MP3Assistant
{
    public class ReversiblePropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value.GetType();

            if (valueType == typeof(int))
            {
                if (((int)value).Equals(0))
                    return "";
                else
                    return value.ToString();
            }
            else if (valueType == typeof(uint))
            {
                if (((uint)value).Equals(0U))
                    return "";
                else
                    return value.ToString();
            }
            else if (valueType == typeof(short))
            {
                if (((short)value).Equals(0))
                    return "";
                else
                    return value.ToString();
            }
            else if (valueType == typeof(long))
            {
                if (((long)value).Equals(0L))
                    return "";
                else
                    return value.ToString();
            }
            else if (valueType == typeof(string[]))
            {
                var array = (string[])value;

                return (array.Count() == 1) ? 
                        array[0] : 
                        string.Join("; ", array);
            }
            else
            {
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = (string)value;

            if (targetType == typeof(int))
            {
                int result;
                if (!Int32.TryParse(stringValue, out result))
                    result = 0;

                return result;
            }
            else if (targetType == typeof(uint))
            {
                uint result;
                if (!UInt32.TryParse(stringValue, out result))
                    result = 0;

                return result;
            }
            else if (targetType == typeof(short))
            {
                short result;
                if (!Int16.TryParse(stringValue, out result))
                    result = 0;

                return result;
            }
            else if (targetType == typeof(string[]))
            {
                var text = stringValue.Trim(new[] { ' ', ';' });

                return text.Split(';').ToList().Select(s => s.Trim()).ToArray();
            }
            else
            {
                return value;
            }
        }
    }
}
