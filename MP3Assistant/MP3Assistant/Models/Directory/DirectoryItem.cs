using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using TagLib;


namespace MP3Assistant
{
    /// <summary>
    /// Directory item model
    /// </summary>
    public class DirectoryItem : INotifyPropertyChanged
    {
        #region Private Members

        private PropertyInfo[] _attributes;
        private string _extension;
        private File _tagFile;

        private static readonly double _expirationSeconds = 50d;
        private static readonly MemoryCache _cache = new MemoryCache("DirectoryItemCache");
        private static Dictionary<DirectoryItem, IList<object>> _registeredStorages = new Dictionary<DirectoryItem, IList<object>>();
        private static IAttributeConverter _stringConverter = new BasicStringAttributeConverter();
        private static IAttributeConverter _stringArrayConverter = new BasicStringArrayAttributeConverter();
        private static IAttributeConverter _uintConverter = new BasicUintAttributeConverter();
        private static IAttributeConverter _pictureArrayConverter = new BasicPictureArrayAttributeConverter();
        private static Func<object, object, bool> _areEqualString = (s1, s2) => { return (string)s1 == (string)s2; };
        private static Func<object, object, bool> _areEqualStringArray = (a1, a2) => { return (string[])a1 == (string[])a2; };
        private static Func<object, object, bool> _areEqualUint = (u1, u2) => { return (uint)u1 == (uint)u2; };
        private static Func<object, object, bool> _areEqualPictureArray = (a1, a2) => { return (IPicture[])a1 == (IPicture[])a2; };

        #endregion

        #region Public Properties

        public static ObservableCollection<DirectoryItem> ModifiedItems { get; private set; }

        public ObservableCollection<DirectoryItemAttribute> ModifiedAttributes { get; private set; }

        public DirectoryType Type { get; set; }
        public string FullPath { get; set; }
        public DirectoryItemAttribute ShortName { get; private set; }
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

        public DirectoryItemAttribute Title { get; private set; }
        public DirectoryItemAttribute Performers { get; private set; }
        public DirectoryItemAttribute AlbumPerformers { get; private set; }
        public DirectoryItemAttribute Album { get; private set; }
        public DirectoryItemAttribute Year { get; private set; }
        public DirectoryItemAttribute TrackIndex { get; private set; }
        public DirectoryItemAttribute TrackCount { get; private set; }
        public DirectoryItemAttribute Genres { get; private set; }
        public DirectoryItemAttribute Images { get; private set; }

        public long Length { get; private set; }
        public ushort Bitrate { get; private set; }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Constructors

        static DirectoryItem()
        {
            ModifiedItems = new ObservableCollection<DirectoryItem>();
        }

        private DirectoryItem(string fullPath)
        {
            // Get the directory type
            Type = DirectoryHelpers.GetDirectoryType(fullPath);
            // Store the full path, short name and extension
            FullPath = fullPath;
            if (Type == DirectoryType.File || Type == DirectoryType.MP3File)
            {
                ShortName = new DirectoryItemAttribute("Nazwa", DirectoryHelpers.GetDirectoryName(FullPath, false), _stringConverter, _areEqualString);
                _extension = DirectoryHelpers.GetExtension(FullPath);
            }
            else
                ShortName = new DirectoryItemAttribute("Nazwa", DirectoryHelpers.GetDirectoryName(FullPath), _stringConverter, _areEqualString);

            // Determine if is a hidden directory (drives seem to be marked as hidden, so let's prevent that)
            Hidden = Type != DirectoryType.Drive && DirectoryHelpers.IsHidden(FullPath);

            if (Type == DirectoryType.MP3File)
            {
                _attributes = GetType().GetProperties()
                             .Where(p => p.PropertyType == typeof(DirectoryItemAttribute))
                             .ToArray();
                ModifiedAttributes = new ObservableCollection<DirectoryItemAttribute>();

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

        #region Public Instance Methods

        public void SaveChanges()
        {
            var attributesModified = ModifiedAttributes.Where(a => a != ShortName).Count();

            if (attributesModified > 0)
            {
                _tagFile.Save();
            }

            if (ShortName.HasChanged)
            {
                // Change filename
                var newPath = DirectoryHelpers.SubstituteFilename(FullPath, Name);
                DirectoryHelpers.RenameFile(FullPath, newPath);
                // Update members
                FullPath = newPath;
                _tagFile.Dispose();
                _tagFile = File.Create(FullPath);
                // Increment the counter
                attributesModified += 1;
            }

            // Discard modifications history
            // (iterating backwards as the list is expected to be shrinking accordingly)
            if (attributesModified > 0)
            {
                for (int i = attributesModified - 1; i >= 0; --i)
                {
                    ModifiedAttributes[i].Reset();
                }
            }
        }

        #endregion

        #region Private Instance Methods

        private void SetMP3Info()
        {
            _tagFile = File.Create(FullPath);

            Length = _tagFile.Length;
            Bitrate = (ushort)_tagFile.Properties.AudioBitrate;

            Tag tag = _tagFile.Tag;

            Title = new DirectoryItemAttribute("Tytuł", tag.Title, _stringConverter, _areEqualString);
            Performers = new DirectoryItemAttribute("Wykonawca", tag.Performers, _stringArrayConverter, _areEqualStringArray);
            AlbumPerformers = new DirectoryItemAttribute("Wykonawca albumu", tag.AlbumArtists, _stringArrayConverter, _areEqualStringArray);
            Album = new DirectoryItemAttribute("Album", tag.Album, _stringConverter, _areEqualString);
            Year = new DirectoryItemAttribute("Rok", tag.Year, _uintConverter, _areEqualUint);
            TrackIndex = new DirectoryItemAttribute("Nr ścieżki", tag.Track, _uintConverter, _areEqualUint);
            TrackCount = new DirectoryItemAttribute("Liczba ścieżek", tag.TrackCount, _uintConverter, _areEqualUint);
            Genres = new DirectoryItemAttribute("Gatunek", tag.Genres, _stringArrayConverter, _areEqualStringArray);
            Images = new DirectoryItemAttribute("Obrazy", tag.Pictures, _pictureArrayConverter, _areEqualPictureArray);
        }

        private void SubscribeToPropertyChanges()
        {
            foreach (var attr in _attributes.Select(a => a.GetValue(this) as DirectoryItemAttribute))
            {
                attr.ValueChanged += OnAttributeChanged;
                attr.ValueReset += OnAttributeReset;
            }

            //Images.CollectionChanged += OnImagesChanged;
            ModifiedAttributes.CollectionChanged += OnModifiedAttributesChanged;
        }

        private void OnAttributeChanged(object sender, DirectoryItemAttributeEventArgs e)
        {
            var attr = sender as DirectoryItemAttribute;

            if (attr.Name == "Tytuł")
            {
                _tagFile.Tag.Title = (string)e.NewValue;
            }
            else if (attr.Name == "Wykonawca")
            {
                _tagFile.Tag.Performers = null;
                _tagFile.Tag.Performers = (string[])e.NewValue;
            }
            else if (attr.Name == "Album")
            {
                _tagFile.Tag.Album = (string)e.NewValue;
            }
            else if (attr.Name == "Wykonawca albumu")
            {
                _tagFile.Tag.AlbumArtists = null;
                _tagFile.Tag.AlbumArtists = (string[])e.NewValue;
            }
            else if (attr.Name == "Rok")
            {
                _tagFile.Tag.Year = (uint)e.NewValue;
            }
            else if (attr.Name == "Nr ścieżki")
            {
                _tagFile.Tag.Track = (uint)e.NewValue;
            }
            else if (attr.Name == "Liczba ścieżek")
            {
                _tagFile.Tag.TrackCount = (uint)e.NewValue;
            }
            else if (attr.Name == "Gatunek")
            {
                _tagFile.Tag.Genres = null;
                _tagFile.Tag.Genres = (string[])e.NewValue;
            }
            else if (attr.Name == "Obrazy")
            {
                _tagFile.Tag.Pictures = null;
                _tagFile.Tag.Pictures = (IPicture[])e.NewValue;
            }

            if (ModifiedAttributes.Contains(attr))
            {
                if (!attr.HasChanged)
                    ModifiedAttributes.Remove(attr);
            }
            else
            {
                if (attr.HasChanged)
                    ModifiedAttributes.Add(attr);
            }
        }

        private void OnAttributeReset(object sender, DirectoryItemAttributeEventArgs e)
        {
            var attr = sender as DirectoryItemAttribute;

            ModifiedAttributes.Remove(attr);
        }

        private void OnImagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var action = e.Action;

            if (action == NotifyCollectionChangedAction.Add)
            {
                IPicture[] newArray = _tagFile.Tag.Pictures;
                newArray.Concat(e.NewItems.Cast<byte[]>().Select(p => new Picture(new ByteVector(p))).ToArray());

                _tagFile.Tag.Pictures = null;
                _tagFile.Tag.Pictures = newArray;
            }
        }

        private void OnModifiedAttributesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var action = e.Action;

            // If an attribute has been added to modifications list...
            if (action == NotifyCollectionChangedAction.Add)
            {
                // If it is the first change made to the item...
                if (!ModifiedItems.Contains(this))
                {
                    // Add it to the list of modified items
                    ModifiedItems.Add(this);
                    // Notify the cache it is now part of that list
                    RegisterExternalStorage(this, ModifiedItems);
                }
            }

            // If an attribute has been removed from modifications list...
            if (action == NotifyCollectionChangedAction.Remove)
            {
                // If no changes in the item are left...
                if (ModifiedAttributes.Count == 0)
                {
                    // Remove item from the list of modified items
                    ModifiedItems.Remove(this);
                    // Notify the cache that this list does not contain it anymore
                    UnregisterExternalStorage(this, ModifiedItems);
                }
            }
        }

        #endregion

        #region Private Static Methods

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

        private static void UnregisterExternalStorage(DirectoryItem item, object storage)
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
