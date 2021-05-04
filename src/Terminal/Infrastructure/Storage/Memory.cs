using System;
using Microsoft.Extensions.Caching.Memory;
using Terminal.Core.Exceptions;

namespace Terminal.Infrastructure.Storage
{
    /// <summary>
    /// Simple in memory storage
    /// </summary>
    public class Memory : IStorage
    {
        private const byte CacheTimeToLiveInMin = 30;

        private readonly IMemoryCache _cache;

        public Memory(IMemoryCache cache) => _cache = cache;

        public void Save<TData>(string key, TData data)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(
                TimeSpan.FromMinutes(CacheTimeToLiveInMin)
            );

            _cache.Set(key, data, cacheEntryOptions);
        }

        public TData Get<TData>(string key)
        {
            var isFound = _cache.TryGetValue(key, out TData data);
            if (isFound == false)
                throw new NotFoundException($"Data not found with key:{key}");

            return data;
        }
    }
}
