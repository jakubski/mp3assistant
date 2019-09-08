using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class DirectoryItemModification
    {
        private readonly DirectoryItem _directoryItem;
        private readonly ReversibleProperty<object> _property;
        private readonly object _oldValue;
        private readonly object _newValue;

        public DirectoryItem DirectoryItem
        {
            get { return _directoryItem; }
        }

        public string Property
        {
            get { return _property.Name; }
        }

        public string OldValue
        {
            get { return _oldValue.ToString(); }
        }

        public string NewValue
        {
            get { return _newValue.ToString(); }
        }

        public DirectoryItemModification(DirectoryItem directoryItem, ReversibleProperty<object> property,
                                         object oldValue, object newValue)
        {
            _directoryItem = directoryItem;
            _property = property;
            _oldValue = oldValue;
            _newValue = newValue;
        }
    }
}
