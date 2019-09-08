using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MP3Assistant
{
    public class ReversibleProperty<T>
    {
        private T _initialValue;
        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                var oldValue = _value;

                _value = value;

                ValueChanged?.Invoke(this, new ReversiblePropertyChangedEventArgs()
                {
                    OldValue = oldValue,
                    NewValue = value
                });
            }
        }
        public string Name { get; private set; }
        public bool HasChanged
        {
            get { return ! Value.Equals(_initialValue); }
        }

        public delegate void ReversiblePropertyChangedEventHandler(object sender, ReversiblePropertyChangedEventArgs e);

        public event ReversiblePropertyChangedEventHandler ValueChanged = (sender, e) => { };

        public ReversibleProperty(string name, T initialValue)
        {
            _initialValue = initialValue;
            _value = initialValue;
            Name = name;
        }

        public void Revert()
        {
            _value = _initialValue;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
