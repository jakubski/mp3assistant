using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class DirectoryItemViewModel : INotifyPropertyChanged
    {
        #region Private Members

        private DirectoryItem _directoryItem;
        private readonly RelayCommand<object> _itemExpandedCommand;

        #endregion

        #region Public Properties

        public DirectoryType Type
        {
            get { return _directoryItem.Type; }
        }

        public string FullPath
        {
            get { return _directoryItem.FullPath; }
        }

        public string Name
        {
            get { return _directoryItem.Name; }
        }

        public string Title
        {
            get { return _directoryItem.Title; }
            set { _directoryItem.Title = value; }
        }

        public string[] Performers
        {
            get { return _directoryItem.Performers; }
            set { _directoryItem.Performers = value; }
        }

        public string[] AlbumPerformers
        {
            get { return _directoryItem.AlbumPerformers; }
            set { _directoryItem.AlbumPerformers = value; }
        }

        public string Album
        {
            get { return _directoryItem.Album; }
            set { _directoryItem.Album = value; }
        }

        public uint Year
        {
            get { return _directoryItem.Year; }
            set { _directoryItem.Year = value; }
        }

        public uint TrackIndex
        {
            get { return _directoryItem.TrackIndex; }
            set { _directoryItem.TrackIndex = value; }
        }

        public uint TrackCount
        {
            get { return _directoryItem.TrackCount; }
            set { _directoryItem.TrackCount = value; }
        }

        public string[] Genres
        {
            get { return _directoryItem.Genres; }
            set { _directoryItem.Genres = value; }
        }

        public long Length
        {
            get { return _directoryItem.Length; }
            set { _directoryItem.Length = value; }
        }

        public ushort Bitrate
        {
            get { return _directoryItem.Bitrate; }
            set { _directoryItem.Bitrate = value; }
        }

        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }

        public bool CanExpand
        {
            get { return (Children != null) && (_directoryItem.SubdirectoriesCount > 0); }
        }

        public bool IsExpanded
        {
            get
            { return Children?.Count(c => c != null) > 0; }

            set
            {
                if (value == true)
                    SetChildren();
                else
                    ClearChildren();
            }
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Constructors

        public DirectoryItemViewModel(string fullPath)
        {
            _directoryItem = new DirectoryItem(fullPath);

            if (_directoryItem.SubdirectoriesCount > 0)
                // If it contains anything, initiate the children collection with a placeholder item so it can be expandable
                Children = new ObservableCollection<DirectoryItemViewModel>(new List<DirectoryItemViewModel> { null });
        }        

        #endregion

        private void SetChildren()
        {
            var subdirectories = DirectoryHelpers.GetContents(FullPath);

            Children = new ObservableCollection<DirectoryItemViewModel>(subdirectories.Select(path => new DirectoryItemViewModel(path)));
        }

        private void ClearChildren()
        {
            Children = new ObservableCollection<DirectoryItemViewModel>();

            if (_directoryItem.SubdirectoriesCount > 0)
                Children.Add(null);
        }
    }
}
