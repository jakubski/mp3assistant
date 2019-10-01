using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MP3Assistant
{
    public class FileExplorerColumnViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Name of the column
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        ///  Width of the column
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Name of property being bound
        /// </summary>
        public string BoundProperty { get; set; }
        /// <summary>
        /// Bound value converter
        /// </summary>
        public IValueConverter Converter { get; set; }
        /// <summary>
        /// Type of template to be applied to column
        /// </summary>
        public ColumnTemplate Template { get; set; }
        /// <summary>
        /// If the column should be visible
        /// </summary>
        public bool IsVisible { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
