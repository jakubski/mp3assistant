using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MP3Assistant
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(ViewModel dataContext)
            : this()
        {
            DataContext = dataContext;
        }
    }
}
