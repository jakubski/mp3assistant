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
            get
            {
                if (Trimmed)
                    return _directoryItem.ShortName;
                else
                    return _directoryItem.Name;
            }
        }

        public bool Trimmed { get; set; }

        public bool Hidden
        {
            get { return _directoryItem.Hidden; }
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