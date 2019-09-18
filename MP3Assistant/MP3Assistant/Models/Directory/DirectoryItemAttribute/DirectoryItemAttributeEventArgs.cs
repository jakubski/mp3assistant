using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class DirectoryItemAttributeEventArgs : EventArgs
    {
        public object NewValue { get; set; }
    }
}
