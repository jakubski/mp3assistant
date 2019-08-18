using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    class DirectoryTreeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public DirectoryTreeViewModel()
        {
            Items = new ObservableCollection<DirectoryItemViewModel>();
            Items.Add(new DirectoryItemViewModel(@"C:\"));

            /*foreach (var drive in Directory.GetLogicalDrives())
            {
                Items.Add(new DirectoryItemViewModel(drive));
            }*/
        }
    }
}
