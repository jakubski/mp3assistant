using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class ReversiblePropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}
