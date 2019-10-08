using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    public class AggregatedDirectoryItemViewModel : ViewModel
    {
        private DirectoryItemViewModel[] _items;

        public int ItemCount => _items.Count();

        public AggregatedDirectoryItemAttributeViewModel ShortName { get; }
        public AggregatedDirectoryItemAttributeViewModel Title { get; }
        public AggregatedDirectoryItemAttributeViewModel Performers { get; }
        public AggregatedDirectoryItemAttributeViewModel AlbumPerformers { get; }
        public AggregatedDirectoryItemAttributeViewModel Album { get; }
        public AggregatedDirectoryItemAttributeViewModel Year { get; }
        public AggregatedDirectoryItemAttributeViewModel TrackIndex { get; }
        public AggregatedDirectoryItemAttributeViewModel TrackCount { get; }
        public AggregatedDirectoryItemAttributeViewModel Genres { get; }
        public AggregatedDirectoryItemAttributeViewModel Images { get; }
        
        public int ImageCount => _items[0].ImageCount;
        public int CurrentImageIndex
        {
            get { return _items[0].CurrentImageIndex; }
            set { _items[0].CurrentImageIndex = value; }
        }
        public byte[] CurrentImage => Images.AreValuesDifferent ? DirectoryItem.MultipleImagesPlaceholder : _items[0].CurrentImage;

        public AggregatedDirectoryItemViewModel(IEnumerable<DirectoryItemViewModel> items)
        {
            _items = items.ToArray();
            
            ShortName = new AggregatedStringAttributeViewModel(_items.Select(item => item.ShortName));
            Title = new AggregatedStringAttributeViewModel(_items.Select(item => item.Title));
            Performers = new AggregatedStringAttributeViewModel(_items.Select(item => item.Performers));
            Album = new AggregatedStringAttributeViewModel(_items.Select(item => item.Album));
            AlbumPerformers = new AggregatedStringAttributeViewModel(_items.Select(item => item.AlbumPerformers));
            Year = new AggregatedNumberAttributeViewModel(_items.Select(item => item.Year));
            TrackIndex = new AggregatedNumberAttributeViewModel(_items.Select(item => item.TrackIndex));
            TrackCount = new AggregatedNumberAttributeViewModel(_items.Select(item => item.TrackCount));
            Genres = new AggregatedStringAttributeViewModel(_items.Select(item => item.Genres));
            Images = new AggregatedIPictureArrayAttributeViewModel(_items.Select(item => item.Images));
        }
    }
}
