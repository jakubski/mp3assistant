using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class FileExplorerViewModel : INotifyPropertyChanged
    {
        private List<DirectoryItemViewModel> _contents;
        private Stack<string> _backwardPathHistory;
        private Stack<string> _forwardPathHistory;

        public string CurrentPath { get; set; }

        public bool HideHiddenContents { get; set; }

        public ObservableCollection<string> SuggestedPaths { get; private set; }
        public ObservableCollection<DirectoryItemViewModel> Contents
        {
            get
            {
                var contents = _contents;

                if (HideHiddenContents)
                    contents = contents.Where(file => !file.Hidden).ToList();

                return new ObservableCollection<DirectoryItemViewModel>(contents);
            }

            set
            {
                _contents = value.ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public VoidRelayCommand BackButtonClickCommand { get; private set; }
        public VoidRelayCommand NextButtonClickCommand { get; private set; }
        public RelayCommand<DirectoryItemViewModel> ItemDoubleClickCommand { get; private set; }

        public FileExplorerViewModel()
        {
            // Initialize fields
            _backwardPathHistory = new Stack<string>();
            _forwardPathHistory = new Stack<string>();

            HideHiddenContents = true;

            SuggestedPaths = new ObservableCollection<string>(new string[] { "" });          

            BackButtonClickCommand = new VoidRelayCommand(GoToPreviousLocation);
            NextButtonClickCommand = new VoidRelayCommand(GoToNextLocation);
            ItemDoubleClickCommand = new RelayCommand<DirectoryItemViewModel>(Item_DoubleClick);

            // Set the initial path
            SetLocation("\\");
        }

        private void SetLocation(string newPath)
        {
            List<string> contents;

            if (newPath == "\\")
                contents = DirectoryHelpers.GetRootDirectoryContents();
            else
                contents = DirectoryHelpers.GetContents(newPath);

            CurrentPath = newPath;
            Contents = new ObservableCollection<DirectoryItemViewModel>(contents.Select(path => new DirectoryItemViewModel(path)));
            SuggestedPaths[0] = CurrentPath;
        }

        /// <summary>
        /// Moves to a specified directory
        /// </summary>
        /// <param name="newPath"></param>
        private void GoToNewLocation(string newPath)
        {
            _backwardPathHistory.Push(CurrentPath);
            _forwardPathHistory.Clear();
            SetLocation(newPath);
        }

        /// <summary>
        /// Back out to a previously visited directory
        /// </summary>
        private void GoToPreviousLocation()
        {
            if (_backwardPathHistory.Count > 0)
            {
                var currentPath = CurrentPath;
                var newPath = _backwardPathHistory.Pop();

                _forwardPathHistory.Push(currentPath);
                SetLocation(newPath);
            }
        }

        /// <summary>
        /// Moves to a directory from which the user has backed out
        /// </summary>
        private void GoToNextLocation()
        {
            if (_forwardPathHistory.Count > 0)
            {
                var currentPath = CurrentPath;
                var newPath = _forwardPathHistory.Pop();

                _backwardPathHistory.Push(currentPath);
                SetLocation(newPath);
            }
        }
        
        private void Item_DoubleClick(DirectoryItemViewModel item)
        {
            var type = item.Type;

            switch (type)
            {
                // If double clicked on a folder or a drive..
                case DirectoryType.Drive:
                case DirectoryType.Folder:
                    // ...Enter the directory
                    GoToNewLocation(item.FullPath);
                    break;
                // If double clicked on a file
                case DirectoryType.File:
                case DirectoryType.MP3File:
                    return;
            }
        }
    }
}
