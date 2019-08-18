using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class BaseDirectoryViewPageViewModel : INotifyPropertyChanged
    {
        public ApplicationPage DirectoryFrameContent { get; set; }
        public ApplicationPage MainFrameContent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public BaseDirectoryViewPageViewModel()
        {
            this.DirectoryFrameContent = ApplicationPage.DirectoryTree;
            this.MainFrameContent = ApplicationPage.Blank;
        }
    }
}
