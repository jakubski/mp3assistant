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
            get { return HideExtension ? ShortName.Value : _directoryItem.Name; }
        }

        public ReversibleProperty<string> ShortName
        {
            get { return _directoryItem.ShortName; }
            set { _directoryItem.ShortName = value; }
        }

        public bool HideExtension { get; set; }

        public bool Hidden
        {
            get { return _directoryItem.Hidden; }
        }

        public ReversibleProperty<string> Title
        {
            get { return _directoryItem.Title; }
            set { _directoryItem.Title = value; }
        }

        public ReversibleProperty<string[]> Performers
        {
            get { return _directoryItem.Performers; }
            set { _directoryItem.Performers = value; }
        }

        public ReversibleProperty<string[]> AlbumPerformers
        {
            get { return _directoryItem.AlbumPerformers; }
            set { _directoryItem.AlbumPerformers = value; }
        }

        public ReversibleProperty<string> Album
        {
            get { return _directoryItem.Album; }
            set { _directoryItem.Album = value; }
        }

        public ReversibleProperty<uint> Year
        {
            get { return _directoryItem.Year; }
            set { _directoryItem.Year = value; }
        }

        public ReversibleProperty<uint> TrackIndex
        {
            get { return _directoryItem.TrackIndex; }
            set { _directoryItem.TrackIndex = value; }
        }

        public ReversibleProperty<uint> TrackCount
        {
            get { return _directoryItem.TrackCount; }
            set { _directoryItem.TrackCount = value; }
        }

        public ReversibleProperty<string[]> Genres
        {
            get { return _directoryItem.Genres; }
            set { _directoryItem.Genres = value; }
        }

        public List<byte[]> Images
        {
            get { return _directoryItem.Images?.ToList(); }
            set { _directoryItem.Images = value.ToArray(); }
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

        #endregion

        #region Constructors

        public DirectoryItemViewModel(string fullPath)
        {
            _directoryItem = DirectoryItemFactory.GetOrCreate(fullPath);
        }

        #endregion
    }
}