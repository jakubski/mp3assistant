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
        private readonly string _propertyName;
        private readonly object _oldValue;
        private readonly object _newValue;

        public DirectoryItem DirectoryItem
        {
            get { return _directoryItem; }
        }

        public string Property
        {
            get { return _propertyName; }
        }

        public object OldValue
        {
            get { return _oldValue; }
        }

        public object NewValue
        {
            get { return _newValue; }
        }

        public DirectoryItemModification(DirectoryItem directoryItem, string propertyName,
                                         object oldValue, object newValue)
        {
            _directoryItem = directoryItem;
            _propertyName = propertyName;
            _oldValue = oldValue;
            _newValue = newValue;
        }
    }
}
