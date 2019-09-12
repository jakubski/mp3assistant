using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class ModifiedDirectoryItemViewModel : ViewModel
    {
        public DirectoryItemViewModel Item { get; private set; }
        public ObservableCollection<DirectoryItemModification> Modifications { get; private set; }

        public ModifiedDirectoryItemViewModel(DirectoryItem directoryItem, IEnumerable<DirectoryItemModification> modifications)
        {
            Item = new DirectoryItemViewModel(directoryItem.FullPath);
            Modifications = new ObservableCollection<DirectoryItemModification>();

            foreach (var uniqueProperty in modifications.Select(m => m.Property).Distinct())
            {
                Modifications.Add(modifications.Last(m => m.Property == uniqueProperty));
            }
        }
    }
}
