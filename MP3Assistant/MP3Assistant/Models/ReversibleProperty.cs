using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MP3Assistant
{
    public class ReversibleProperty<T>
    {
        private List<T> _history = new List<T>();

        private T _InitialValue
        {
            get
            {
                return _history.First();
            }
        }

        public T Value
        {
            get
            {
                return _history.Last();
            }

            set
            {
                _history.Add(value);
            }
        }

        public bool HasChanged
        {
            get
            {
                return Value.Equals(_history.First());
            }
        }

        public ReversibleProperty(T initialValue)
        {
            Value = initialValue;
        }

        public void Revert()
        {
            var length = _history.Count;

            if (length > 0)
            {
                _history.RemoveAt(length - 1);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
