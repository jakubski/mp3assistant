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
        private Func<object, object, bool> _equalsFunc;
        private IAttributeConverter _converter;

        public delegate void DirectoryItemAttributeEventHandler(object sender, DirectoryItemAttributeEventArgs e);
        public event DirectoryItemAttributeEventHandler ValueChanged;
        public event DirectoryItemAttributeEventHandler ValueReset;

        public object Value
        {
            get { return _converter.ForView(_currentValue); }
            set
            {
                _currentValue = _converter.FromView(value);

                ValueChanged?.Invoke(this, new DirectoryItemAttributeEventArgs()
                                           { NewValue = _currentValue });
            }
        }

        public object InitialValue
        {
            get { return _converter.ForView(_initialValue); }
        }

        public string Name { get; private set; }

        public bool HasChanged
        {
            get { return ! _equalsFunc(_currentValue, _initialValue); }
        }

        public DirectoryItemAttribute(string name, object initialValue, IAttributeConverter converter, Func<object, object, bool> equalsFunc)
        {
            _initialValue = initialValue;
            _currentValue = initialValue;
            _converter = converter;
            _equalsFunc = equalsFunc;

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
            return Value.ToString();
        }

        
    }
}
