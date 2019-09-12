using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using TagLib;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MP3Assistant
{
    /// <summary>
    /// Directory item model
    /// </summary>
    public class DirectoryItem : INotifyPropertyChanged
    {
        #region Private Members

        private string _extension;
        private File _tagFile;
        private static readonly double _expirationSeconds = 50d;
        private static readonly MemoryCache _cache = new MemoryCache("DirectoryItemFCache");
        private static Dictionary<DirectoryItem, IList<object>> _registeredStorages = new Dictionary<DirectoryItem, IList<object>>();

        #endregion

        #region Public Properties

        public static ObservableCollection<DirectoryItemModification> Modifications { get; private set; }

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

        static DirectoryItem()
        {
            Modifications = new ObservableCollection<DirectoryItemModification>();
            Modifications.CollectionChanged += OnModificationsChanged;
        }

        private DirectoryItem(string fullPath)
        {
            // Get the directory type
            Type = DirectoryHelpers.GetDirectoryType(fullPath);
            // Store the full path, short name and extension
            FullPath = fullPath;
            if (Type == DirectoryType.File || Type == DirectoryType.MP3File)
            {
                ShortName = new ReversibleProperty<string>("Nazwa", DirectoryHelpers.GetDirectoryName(FullPath, false));
                _extension = DirectoryHelpers.GetExtension(FullPath);
            }
            else
                ShortName = new ReversibleProperty<string>("Nazwa", DirectoryHelpers.GetDirectoryName(FullPath));

            // Determine if is a hidden directory (drives seem to be marked as hidden, so let's prevent that)
            Hidden = Type != DirectoryType.Drive && DirectoryHelpers.IsHidden(FullPath);

            if (Type == DirectoryType.MP3File)
            {
                SetMP3Info();
                SubscribeToPropertyChanges();
            }
        }

        #endregion

        #region Public Static Methods

        public static DirectoryItem GetOrCreate(string path)
        {
            DirectoryItem item = _cache[path] as DirectoryItem;

            if (item == null)
            {
                item = new DirectoryItem(path);

                if (item.Type == DirectoryType.File || item.Type == DirectoryType.MP3File)
                {
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.SlidingExpiration = TimeSpan.FromSeconds(_expirationSeconds);
                    policy.ChangeMonitors.Add(new HostFileChangeMonitor(new[] { path }.ToList()));

                    _cache.Set(path, item, policy);
                }
            }

            return item;
        }

        #endregion

        #region Private Instance Methods

        private void SetMP3Info()
        {
            _tagFile = File.Create(FullPath);

            Length = _tagFile.Length;
            Bitrate = (ushort)_tagFile.Properties.AudioBitrate;

            Tag tag = _tagFile.Tag;

            Title = new ReversibleProperty<string>("Tytuł", tag.Title);
            Performers = new ReversibleProperty<string[]>("Wykonawca", tag.Performers);
            AlbumPerformers = new ReversibleProperty<string[]>("Wykonawca albumu", tag.AlbumArtists);
            Album = new ReversibleProperty<string>("Album", tag.Album);
            Year = new ReversibleProperty<uint>("Rok", tag.Year);
            TrackIndex = new ReversibleProperty<uint>("Nr ścieżki", tag.Track);
            TrackCount = new ReversibleProperty<uint>("Liczba ścieżek", tag.TrackCount);
            Genres = new ReversibleProperty<string[]>("Gatunek", tag.Genres);
            Images = tag.Pictures.Select(image => image.Data.Data).ToArray();
        }

        private void SubscribeToPropertyChanges()
        {
            ShortName.ValueChanged += OnPropertyChanged;
            Title.ValueChanged += OnPropertyChanged;
            Performers.ValueChanged += OnPropertyChanged;
            AlbumPerformers.ValueChanged += OnPropertyChanged;
            Album.ValueChanged += OnPropertyChanged;
            Year.ValueChanged += OnPropertyChanged;
            TrackIndex.ValueChanged += OnPropertyChanged;
            TrackCount.ValueChanged += OnPropertyChanged;
            Genres.ValueChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, ReversiblePropertyChangedEventArgs e)
        {
            var modification = new DirectoryItemModification(this, e.PropertyName, e.OldValue, e.NewValue);

            Modifications.Add(modification);
        }

        #endregion

        #region Private Static Methods

        private static void OnModificationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var action = e.Action;

            // If an item has been added to modifications list...
            if (action == NotifyCollectionChangedAction.Add)
            {
                var addedItems = e.NewItems.Cast<DirectoryItemModification>();

                // Notify the cache it is a part of the list
                foreach (var modification in addedItems)
                    RegisterExternalStorage(modification.DirectoryItem, Modifications);
            }

            // If an item has been removed from modifications list...
            if (action == NotifyCollectionChangedAction.Remove)
            {
                var removedItems = e.OldItems.Cast<DirectoryItemModification>();

                // Notify the cache it is not a part of this list anymore
                foreach (var modification in removedItems)
                    UpdateExternalStorage(modification.DirectoryItem, Modifications);
            }
        }

        private static void RegisterExternalStorage(DirectoryItem item, object storage)
        {
            bool storagesRegistered = _registeredStorages.ContainsKey(item) && _registeredStorages[item].Count > 0;
            bool itemCached = (_cache[item.FullPath] as DirectoryItem) != null;

            if (!storagesRegistered || !itemCached)
            {
                _registeredStorages.Add(item, new List<object>());

                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new[] { item.FullPath }.ToList()));
                policy.Priority = CacheItemPriority.NotRemovable;

                if (itemCached)
                    _cache.Remove(item.FullPath);

                _cache.Set(item.FullPath, item, policy);
            }

            _registeredStorages[item].Add(storage);
        }

        private static void UpdateExternalStorage(DirectoryItem item, object storage)
        {
            _registeredStorages[item].Remove(storage);

            if (_registeredStorages[item].Count == 0)
            {
                _registeredStorages.Remove(item);

                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new[] { item.FullPath }.ToList()));
                policy.SlidingExpiration = TimeSpan.FromSeconds(_expirationSeconds);

                _cache.Remove(item.FullPath);
                _cache.Set(item.FullPath, item, policy);
            }
        }

        #endregion
    }
}
