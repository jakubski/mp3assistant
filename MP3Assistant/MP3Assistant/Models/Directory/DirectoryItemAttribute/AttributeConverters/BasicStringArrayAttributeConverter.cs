using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class BasicStringArrayAttributeConverter : IAttributeConverter
    {
        public string ForView(object passedValue)
        {
            if (passedValue == null)
                return string.Empty;

            var array = (string[])passedValue;
            string joinedArray;

            if (array.Count() == 1)
                joinedArray = array[0];
            else
                joinedArray = string.Join("; ", array.Select(s => s.Trim()));

            return joinedArray;
        }

        public object FromView(string receivedValue)
        {
            if (receivedValue == null)
                return new string[] { };

            var text = receivedValue;

            if (text == string.Empty)
                return new string[] { };

            string[] splitText;

            text = text.Trim(new[] { ';' });
            splitText = text.Split(';');
            splitText = splitText.Select(s => s.Trim()).ToArray();

            return splitText;
        }
    }
}
