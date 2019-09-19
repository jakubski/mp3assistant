using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public interface IAttributeConverter
    {
        object FromView(object receivedValue);
        object ForView(object passedValue);
    }
}
