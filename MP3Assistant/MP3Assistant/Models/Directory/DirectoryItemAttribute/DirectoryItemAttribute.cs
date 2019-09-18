using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MP3Assistant
{
    public class DirectoryItemAttribute
    {
        private object _initialValue;
        private object _currentValue;
        private IAttributeConverter _converter;

        public delegate void DirectoryItemAttributeEventHandler(object sender, DirectoryItemAttributeEventArgs e);
        public event DirectoryItemAttributeEventHandler ValueChanged;
        public event DirectoryItemAttributeEventHandler ValueReset;

        public string Value
        {
            get { return _converter.ForView(_currentValue); }
            set
            {
                _currentValue = _converter.FromView(value);

                ValueChanged?.Invoke(this, new DirectoryItemAttributeEventArgs()
                                           { NewValue = _currentValue });
            }
        }

        public string InitialValue
        {
            get { return _converter.ForView(_initialValue); }
        }

        public string Name { get; private set; }

        public bool HasChanged
        {
            get { return !(InitialValue == Value); }
        }

        public DirectoryItemAttribute(string name, object initialValue, IAttributeConverter converter)
        {
            _initialValue = initialValue;
            _currentValue = initialValue;
            _converter = converter;

            Name = name;
        }

        public void Revert()
        {
            _currentValue = _initialValue;

            ValueChanged?.Invoke(this, new DirectoryItemAttributeEventArgs()
                                       { NewValue = _currentValue });
        }

        public void Reset()
        {
            _initialValue = _currentValue;

            ValueReset?.Invoke(this, new DirectoryItemAttributeEventArgs());
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
