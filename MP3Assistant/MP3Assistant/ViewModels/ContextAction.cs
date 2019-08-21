using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MP3Assistant
{
    public class ContextAction : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ICommand Action { get; set; }
        public object ActionParameter { get; set; }
        public ObservableCollection<ContextAction> SubActions { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public ContextAction(string name, ICommand action = null, object actionParameter = null, IEnumerable<ContextAction> subitems = null)
        {
            Name = name;
            Action = action;
            ActionParameter = actionParameter;

            if (subitems != null)
                SubActions = new ObservableCollection<ContextAction>(subitems);
        }
    }
}
