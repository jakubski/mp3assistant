using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class AggregatedIPictureArrayAttributeViewModel : AggregatedDirectoryItemAttributeViewModel
    {
        public override object AggregatedValue => throw new NotImplementedException();

        public AggregatedIPictureArrayAttributeViewModel(IEnumerable<DirectoryItemAttribute> attributes)
            : base(attributes) { }
    }
}
