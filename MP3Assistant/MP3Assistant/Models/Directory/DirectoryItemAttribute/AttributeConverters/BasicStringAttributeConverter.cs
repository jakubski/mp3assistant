using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class BasicStringAttributeConverter : IAttributeConverter
    {
        public object ForView(object passedValue)
        {
            if (passedValue == null)
                return string.Empty;

            var text = (string)passedValue;

            return text;
        }

        public object FromView(object receivedValue)
        {
            if (receivedValue == null)
                return string.Empty;

            var text = (string)receivedValue;

            text = text.Trim();

            return text;
        }
    }
}
