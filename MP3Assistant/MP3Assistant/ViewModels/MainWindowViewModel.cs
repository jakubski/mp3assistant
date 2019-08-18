using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ApplicationPage WindowContent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public MainWindowViewModel()
        {
            this.WindowContent = ApplicationPage.MainPage;
        }
    }
}
