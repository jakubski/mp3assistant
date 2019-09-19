using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MP3Assistant
{
    public class DirectoryItemViewModel : ViewModel
    {
        #region Private Members

        private DirectoryItem _directoryItem;

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
            get { return HideExtension ? (string)ShortName.Value : _directoryItem.Name; }
        }

        public DirectoryItemAttribute ShortName
        {
            get { return _directoryItem.ShortName; }
        }

        public bool HideExtension { get; set; }

        public bool Hidden
        {
            get { return _directoryItem.Hidden; }
        }

        public DirectoryItemAttribute Title
        {
            get { return _directoryItem.Title; }
        }

        public DirectoryItemAttribute Performers
        {
            get { return _directoryItem.Performers; }
        }

        public DirectoryItemAttribute AlbumPerformers
        {
            get { return _directoryItem.AlbumPerformers; }
        }

        public DirectoryItemAttribute Album
        {
            get { return _directoryItem.Album; }
        }

        public DirectoryItemAttribute Year
        {
            get { return _directoryItem.Year; }
        }

        public DirectoryItemAttribute TrackIndex
        {
            get { return _directoryItem.TrackIndex; }
        }

        public DirectoryItemAttribute TrackCount
        {
            get { return _directoryItem.TrackCount; }
        }

        public DirectoryItemAttribute Genres
        {
            get { return _directoryItem.Genres; }
        }

        public ObservableCollection<byte[]> Images
        {
            get { return new ObservableCollection<byte[]>((_directoryItem.Images?.Value ?? new List<byte[]>()) as List<byte[]>); }
            set { _directoryItem.Images.Value = new List<byte[]>(value); }
        }
        public int ImageCount { get { return Images.Count; } }
        public int CurrentImageIndex { get; set; }
        public byte[] CurrentImage
        {
            get { return ImageCount == 0 ? null : Images[CurrentImageIndex]; }
        }

        public long Length
        {
            get { return _directoryItem.Length; }
        }

        public ushort Bitrate
        {
            get { return _directoryItem.Bitrate; }
        }

        public ObservableCollection<DirectoryItemAttribute> ModifiedAttributes
        {
            get { return _directoryItem.ModifiedAttributes; }
        }

        #endregion

        #region Constructors

        public DirectoryItemViewModel(string fullPath)
        {
            _directoryItem = DirectoryItem.GetOrCreate(fullPath);

            CurrentImageIndex = 0;
        }

        #endregion

        public void ConfirmModifications()
        {
            _directoryItem.SaveChanges();
        }
    }
}