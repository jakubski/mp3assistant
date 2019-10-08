using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class AggregatedStringAttributeViewModel : AggregatedDirectoryItemAttributeViewModel
    {
        public override object AggregatedValue => "<różne wartości>";

        public AggregatedStringAttributeViewModel(IEnumerable<DirectoryItemAttribute> attributes)
            : base(attributes) { }
    }
}
