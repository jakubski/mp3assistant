using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public interface IAttributeConverter
    {
        object FromView(string receivedValue);
        string ForView(object passedValue);
    }
}
