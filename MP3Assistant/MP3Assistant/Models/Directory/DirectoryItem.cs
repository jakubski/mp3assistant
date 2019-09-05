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
        private string _extension;
        private File _tagFile;

        #region Public Properties

        public DirectoryType Type { get; set; }
        public string FullPath { get; set; }
        public ReversibleProperty<string> ShortName { get; set; }

        public string Name
        {
            get
            {
                return ShortName.Value + _extension;
            }

            set
            {
                ShortName.Value = DirectoryHelpers.GetTrimmedName(value);
            }
        }

        public bool Hidden { get; private set; }

        public ReversibleProperty<string> Title { get; set; }
        public ReversibleProperty<string[]> Performers { get; set; }
        public ReversibleProperty<string[]> AlbumPerformers { get; set; }
        public ReversibleProperty<string> Album { get; set; }
        public ReversibleProperty<uint> Year { get; set; }
        public ReversibleProperty<uint> TrackIndex { get; set; }
        public ReversibleProperty<uint> TrackCount { get; set; }
        public ReversibleProperty<string[]> Genres { get; set; }
        public byte[][] Images { get; set; }
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
            // Store the full path, short name and extension
            FullPath = fullPath;
            if (Type == DirectoryType.File || Type == DirectoryType.MP3File)
            {
                ShortName = new ReversibleProperty<string>(DirectoryHelpers.GetDirectoryName(FullPath, false));
                _extension = DirectoryHelpers.GetExtension(FullPath);
            }
            else
                ShortName = new ReversibleProperty<string>(DirectoryHelpers.GetDirectoryName(FullPath));

            // Determine if is a hidden directory (drives seem to be marked as hidden)
            Hidden = DirectoryHelpers.IsHidden(FullPath) && Type != DirectoryType.Drive;

            if (Type == DirectoryType.MP3File)
                SetMP3Info();
        }

        #endregion

        ~DirectoryItem()
        {

        }

        private void SetMP3Info()
        {
            _tagFile = File.Create(FullPath);

            Length = _tagFile.Length;
            Bitrate = (ushort)_tagFile.Properties.AudioBitrate;

            Tag tag = _tagFile.Tag;

            Title = new ReversibleProperty<string>(tag.Title);
            Performers = new ReversibleProperty<string[]>(tag.Performers);
            AlbumPerformers = new ReversibleProperty<string[]>(tag.AlbumArtists);
            Album = new ReversibleProperty<string>(tag.Album);
            Year = new ReversibleProperty<uint>(tag.Year);
            TrackIndex = new ReversibleProperty<uint>(tag.Track);
            TrackCount = new ReversibleProperty<uint>(tag.TrackCount);
            Genres = new ReversibleProperty<string[]>(tag.Genres);
            Images = tag.Pictures.Select(image => image.Data.Data).ToArray();
        }

        public async Task SaveChanges()
        {
            if (ShortName.HasChanged)
            {
                Rename();
            }
        }

        private void Rename()
        {

        }
    }
}
