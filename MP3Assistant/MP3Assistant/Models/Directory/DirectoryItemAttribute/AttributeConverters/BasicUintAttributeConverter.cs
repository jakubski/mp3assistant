using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class BasicUintAttributeConverter : IAttributeConverter
    {
        public string ForView(object passedValue)
        {
            var number = (uint)passedValue;

            if (number < 1)
                return string.Empty;
            else
                return number.ToString();
        }

        public object FromView(string receivedValue)
        {
            var text = receivedValue;
            uint number;

            if (uint.TryParse(text, out number))
                return number;
            else
                return 0U;
        }
    }
}
