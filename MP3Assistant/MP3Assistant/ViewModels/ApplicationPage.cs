using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public struct ApplicationPage
    {
        public ApplicationPageType Type { get; set; }
        public ViewModel ViewModel { get; set; }
    }
}
