using System;
using System.Linq;
using System.Runtime.Caching;

namespace MP3Assistant
{
    public static class DirectoryItemFactory
    {
        #region Private Members

        private static readonly double _expirationSeconds = 10d;
        private static readonly MemoryCache _cache = new MemoryCache("DirectoryItemFactoryCache");

        #endregion

        #region Public Methods

        public static DirectoryItem GetOrCreate(string path)
        {
            DirectoryItem item = _cache[path] as DirectoryItem;

            if (item == null)
            {
                item = new DirectoryItem(path);

                if (item.Type == DirectoryType.File || item.Type == DirectoryType.MP3File)
                {
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_expirationSeconds);
                    policy.ChangeMonitors.Add(new HostFileChangeMonitor(new[] { path }.ToList()));

                    _cache.Set(path, item, policy);
                }
            }

            return item;
        }

        #endregion
    }
}
