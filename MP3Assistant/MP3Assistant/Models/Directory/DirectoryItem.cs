using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MP3Assistant
{
    /// <summary>
    /// Directory item model
    /// </summary>
    public class DirectoryItem : INotifyPropertyChanged
    {
        private File _tagFile;

        #region Public Properties

        public DirectoryType Type { get; set; }

        public string FullPath { get; set; }

        public string Name
        {
            get { return DirectoryHelpers.GetDirectoryName(FullPath); }
        }

        public string Title { get; set; }
        public string[] Performers { get; set; }
        public string[] AlbumPerformers { get; set; }
        public string Album { get; set; }
        public uint Year { get; set; }
        public uint TrackIndex { get; set; }
        public uint TrackCount { get; set; }
        public string[] Genres { get; set; }
        public long Length { get; set; }
        public ushort Bitrate { get; set; }

        public int SubdirectoriesCount { get; set; }

        public List<DirectoryItem> Children { get; set; }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Constructors

        public DirectoryItem(string fullPath)
        {
            // Get the directory type
            Type = DirectoryHelpers.GetDirectoryType(fullPath);
            // Store the full path
            FullPath = fullPath;

            if (Type == DirectoryType.File || Type == DirectoryType.MP3File)
            {
                // If the item is a file, it will not contain a collection of children...
                Children = null;

                if (Type == DirectoryType.MP3File)
                {
                    SetMP3Info();
                }
            }
            else
            {
                // Check if directory contains anything
                SubdirectoriesCount = DirectoryHelpers.GetContents(FullPath).Count();
            }
        }

        #endregion

        public void SetChildren()
        {
            if (Type == DirectoryType.File &
                SubdirectoriesCount > 0)
            {
                var subdirectories = DirectoryHelpers.GetContents(FullPath);

                Children = new List<DirectoryItem>(subdirectories.Select(path => new DirectoryItem(path)));
            }
        }

        private void SetMP3Info()
        {
            _tagFile = File.Create(FullPath);

            Length = _tagFile.Length;
            Bitrate = (ushort)_tagFile.Properties.AudioBitrate;

            Tag tag = _tagFile.Tag;

            Title = tag.Title;
            Performers = tag.Performers;
            AlbumPerformers = tag.AlbumArtists;
            Album = tag.Album;
            Year = tag.Year;
            TrackIndex = tag.Track;
            TrackCount = tag.TrackCount;
            Genres = tag.Genres;
        }
    }
}
