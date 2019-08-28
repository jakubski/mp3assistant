using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class MainWindowViewModel : ViewModel
    {
        public ApplicationPage WindowContent { get; set; }

        public MainWindowViewModel()
        {
            this.WindowContent = new ApplicationPage()
            {
                Type = ApplicationPageType.MainPage,
                ViewModel = new MainPageViewModel()
            };
        }
    }
}
