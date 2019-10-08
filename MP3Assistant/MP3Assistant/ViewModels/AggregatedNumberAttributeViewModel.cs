using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class AggregatedNumberAttributeViewModel : AggregatedDirectoryItemAttributeViewModel
    {
        public override object AggregatedValue => "<*>";

        public AggregatedNumberAttributeViewModel(IEnumerable<DirectoryItemAttribute> attributes)
            : base(attributes) { }
    }
}
