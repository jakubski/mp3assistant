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

        public string ShortName
        {
            get
            {
                if (Type == DirectoryType.File || Type == DirectoryType.MP3File)
                    return DirectoryHelpers.GetTrimmedName(Name);
                else
                    return Name;
            }
        }

        public bool Hidden { get; private set; }

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
            // Determine if is a hidden directory (drives seem to be marked as hidden)
            Hidden = DirectoryHelpers.IsHidden(FullPath) && Type != DirectoryType.Drive;

            if (Type == DirectoryType.MP3File)
                SetMP3Info();
        }

        #endregion

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
