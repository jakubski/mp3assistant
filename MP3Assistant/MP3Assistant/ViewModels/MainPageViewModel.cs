using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MP3Assistant
{
    public class MainPageViewModel : ViewModel
    {
        private List<DirectoryItemViewModel> _contents;
        private List<FileExplorerColumnViewModel> _columns;
        private Stack<string> _backwardPathHistory;
        private Stack<string> _forwardPathHistory;

        public string CurrentPath { get; set; }

        public string NavigationLabelText
        {
            get
            {
                switch (FileExplorerPage.Type)
                {
                    case ApplicationPageType.FileExplorerPage:
                        return CurrentPath;
                    case ApplicationPageType.SongEditorPage:
                        return SelectedDirectoryItem.FullPath;
                    case ApplicationPageType.ModificationsPage:
                        return "zmian: " + ModifiedItems.Sum(item => item.ModifiedAttributes.Count).ToString();
                    default:
                        return "...";
                }
            }
        }

        public ApplicationPage NavigationBarPage { get; set; }
        public ApplicationPage FileExplorerPage { get; set; }

        public bool HideHiddenContents { get; set; }
        public bool HideExtensions { get; set; }

        //public ObservableCollection<string> SuggestedPaths { get; private set; }
        public ObservableCollection<DirectoryItemViewModel> Contents
        {
            get
            {
                var contents = _contents;

                // Remove hidden contents if necessary
                if (HideHiddenContents)
                    contents = contents.Where(item => !item.Hidden).ToList();

                // Remove file extensions if necessary
                contents.ForEach(item => { item.HideExtension = HideExtensions; });

                return new ObservableCollection<DirectoryItemViewModel>(contents);
            }

            set { _contents = value.ToList(); }
        }

        public ObservableCollection<FileExplorerColumnViewModel> Columns
        {
            get { return new ObservableCollection<FileExplorerColumnViewModel>(_columns.Where(column => column.IsVisible)); }
        }

        public DirectoryItemViewModel SelectedDirectoryItem { get; set; }

        public ObservableCollection<DirectoryItemViewModel> ModifiedItems
        {
            get
            {
                var viewModels = DirectoryItem.ModifiedItems.Select(item => new DirectoryItemViewModel(item.FullPath));
                return new ObservableCollection<DirectoryItemViewModel>(viewModels);
            }
        }

        public ObservableCollection<ContextAction> FileExplorerHeaderContextMenu { get; private set; }

        public delegate void ColumnChangedEventHandler(object sender, ColumnChangedEventArgs e);

        public event ColumnChangedEventHandler ColumnAdded = (sender, e) => { };
        public event ColumnChangedEventHandler ColumnRemoved = (sender, e) => { };

        public VoidRelayCommand BackButtonClickCommand { get; private set; }
        public VoidRelayCommand NextButtonClickCommand { get; private set; }
        public VoidRelayCommand ModificationsButtonClickCommand { get; private set; }
        public VoidRelayCommand ConfirmModificationsButtonClickCommand { get; private set; }
        public RelayCommand<DirectoryItemViewModel> ItemDoubleClickCommand { get; private set; }
        public RelayCommand<FileExplorerColumnViewModel> AddRemoveColumnCommand { get; private set; }
        public RelayCommand<DirectoryItemAttribute> CancelModificationButtonClickCommand { get; private set; }

        public MainPageViewModel()
        {
            // Initialize fields
            _backwardPathHistory = new Stack<string>();
            _forwardPathHistory = new Stack<string>();

            NavigationBarPage = new ApplicationPage()
            {
                Type = ApplicationPageType.MainPageNavigationBar,
                ViewModel = this
            };
            FileExplorerPage = new ApplicationPage()
            {
                Type = ApplicationPageType.FileExplorerPage,
                ViewModel = this
            };

            var revPropToStringConverter = new ReversiblePropertyOfStringArrayToStringConverter();
            var emptyIntConverter = new ReversiblePropertyOfUintToStringConverter();
            _columns = new List<FileExplorerColumnViewModel>(new[]
            {
                new FileExplorerColumnViewModel()
                {
                    Header = "Nazwa",
                    Width = 240,
                    BoundProperty = nameof(DirectoryItemViewModel.Name),
                    IsVisible = true
                },
                new FileExplorerColumnViewModel()
                {
                    Header = "Tytuł",
                    Width = 180,
                    BoundProperty = nameof(DirectoryItemViewModel.Title),
                    IsVisible = true
                },
                new FileExplorerColumnViewModel()
                {
                    Header = "Wykonawca",
                    Width = 160,
                    BoundProperty = nameof(DirectoryItemViewModel.Performers),
                    IsVisible = true
                },
                new FileExplorerColumnViewModel()
                {
                    Header = "Album",
                    Width = 180,
                    BoundProperty = nameof(DirectoryItemViewModel.Album),
                    IsVisible = true
                },
                new FileExplorerColumnViewModel()
                {
                    Header = "Wykonawca albumu",
                    Width = 160,
                    BoundProperty = nameof(DirectoryItemViewModel.AlbumPerformers),
                    IsVisible = true
                },
                new FileExplorerColumnViewModel()
                {
                    Header = "Rok",
                    Width = 50,
                    BoundProperty = nameof(DirectoryItemViewModel.Year),
                    IsVisible = true
                }
            });

            HideHiddenContents = true;
            HideExtensions = true;

            BackButtonClickCommand = new VoidRelayCommand(GoToPreviousLocation);
            NextButtonClickCommand = new VoidRelayCommand(GoToNextLocation);
            ModificationsButtonClickCommand = new VoidRelayCommand(OpenModificationsPage);
            ConfirmModificationsButtonClickCommand = new VoidRelayCommand(ConfirmModifications);
            ItemDoubleClickCommand = new RelayCommand<DirectoryItemViewModel>(Item_DoubleClick);
            AddRemoveColumnCommand = new RelayCommand<FileExplorerColumnViewModel>(AddRemoveColumn);
            CancelModificationButtonClickCommand = new RelayCommand<DirectoryItemAttribute>(CancelModification);

            FileExplorerHeaderContextMenu = new ObservableCollection<ContextAction>(new[]
            {
                new ContextAction("Kolumny", null, null, _columns.Select(column => new ContextAction(column.Header, AddRemoveColumnCommand, column)))
            });

            // Set the initial path
            var parameters = GlobalSettings.CommandLineParameters;
            if (parameters.Length < 1)
                SetLocation("\\");
            else
                SetLocation(parameters[0]);
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
        /// Backs out to a previously visited directory
        /// </summary>
        private void GoToPreviousLocation()
        {
            if (FileExplorerPage.Type == ApplicationPageType.FileExplorerPage)
            {
                if (_backwardPathHistory.Count > 0)
                {
                    var currentPath = CurrentPath;
                    var newPath = _backwardPathHistory.Pop();

                    _forwardPathHistory.Push(currentPath);
                    SetLocation(newPath);
                }
            }
            else if (FileExplorerPage.Type == ApplicationPageType.SongEditorPage
                  || FileExplorerPage.Type == ApplicationPageType.ModificationsPage)
            {
                OpenFileExplorer();
            }
        }

        /// <summary>
        /// Moves to a directory from which the user has backed out
        /// </summary>
        private void GoToNextLocation()
        {
            if (FileExplorerPage.Type == ApplicationPageType.FileExplorerPage)
            {
                if (_forwardPathHistory.Count > 0)
                {
                    var currentPath = CurrentPath;
                    var newPath = _forwardPathHistory.Pop();

                    _backwardPathHistory.Push(currentPath);
                    SetLocation(newPath);
                }
            }
        }

        private void OpenSongEditor()
        {
            FileExplorerPage = new ApplicationPage()
            {
                Type = ApplicationPageType.SongEditorPage,
                ViewModel = this
            };
        }

        private void OpenFileExplorer()
        {
            FileExplorerPage = new ApplicationPage()
            {
                Type = ApplicationPageType.FileExplorerPage,
                ViewModel = this
            };
        }

        private void OpenModificationsPage()
        {
            FileExplorerPage = new ApplicationPage()
            {
                Type = ApplicationPageType.ModificationsPage,
                ViewModel = this
            };
        }

        private void AddRemoveColumn(FileExplorerColumnViewModel column)
        {
            var newVisibility = !column.IsVisible;

            column.IsVisible = newVisibility;

            // Raise event
            var e = new ColumnChangedEventArgs() { Column = column };
            if (newVisibility)
                ColumnAdded?.Invoke(this, e);
            else
                ColumnRemoved?.Invoke(this, e);
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
                    return;
                case DirectoryType.MP3File:
                    OpenSongEditor();
                    break;
            }
        }

        private void ConfirmModifications()
        {
            foreach (var item in ModifiedItems)
                item.ConfirmModifications();
        }

        private void CancelModification(DirectoryItemAttribute attribute)
        {
            attribute.Revert();
        }
    }
}
