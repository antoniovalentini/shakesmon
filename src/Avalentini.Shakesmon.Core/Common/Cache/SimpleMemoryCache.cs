using System;
using Microsoft.Extensions.Caching.Memory;

namespace Avalentini.Shakesmon.Core.Common.Cache
{
    public interface ICache
    {
        TItem Get<TItem>(object key);
        void Set<TItem>(object key, TItem value);
    }

    public class SimpleMemoryCache : ICache
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
 
        public TItem Get<TItem>(object key)
        {
            return _cache.Get<TItem>(key);
        }

        public void Set<TItem>(object key, TItem value)
        {
            _cache.Set(key, value);
        }
    }
}
