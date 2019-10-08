using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public abstract class AggregatedDirectoryItemAttributeViewModel : ViewModel
    {
        protected DirectoryItemAttribute[] _attributes;

        public bool AreValuesDifferent
        {
            get
            {
                var count = _attributes.Count();

                if (count == 1)
                    return false;
                else
                    return _attributes.Distinct().Count() > 1;
            }
        }

        public abstract object AggregatedValue { get; }

        public object ValueForView
        {
            get
            {
                if (AreValuesDifferent)
                    return AggregatedValue;
                else
                    return _attributes[0].ValueForView;
            }

            set
            {
                foreach (var attr in _attributes)
                    attr.ValueForView = value;
            }
        }
        
        public AggregatedDirectoryItemAttributeViewModel(IEnumerable<DirectoryItemAttribute> attributes)
        {
            _attributes = attributes.ToArray();
        }
    }
}
