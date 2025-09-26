using System.Collections.Concurrent;

namespace Z.Ai.Sdk.Core.Cache;

/// <summary>
/// Thread-safe local cache implementation using ConcurrentDictionary. Provides basic caching
/// functionality with expiration support.
/// </summary>
public class LocalCache : ICache
{
    private static readonly ConcurrentDictionary<string, CacheValue> Cache = new();
    private static readonly Lazy<LocalCache> Instance = new(() => new LocalCache());

    private LocalCache()
    {
    }

    /// <summary>
    /// Gets singleton instance of LocalCache.
    /// </summary>
    public static LocalCache GetInstance() => Instance.Value;

    /// <summary>
    /// Retrieves cached value by key.
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>Cached value or empty string if not found or expired</returns>
    public string Get(string key)
    {
        if (Cache.TryGetValue(key, out var cacheValue))
        {
            if (DateTime.UtcNow < cacheValue.Expiration)
            {
                return cacheValue.Value;
            }

            // Remove expired entry
            Cache.TryRemove(key, out _);
        }

        return string.Empty;
    }

    /// <summary>
    /// Sets cache value with expiration time.
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <param name="value">The value to cache</param>
    /// <param name="expire">Expiration duration in seconds</param>
    public void Set(string key, string value, int expire)
    {
        var expiration = DateTime.UtcNow.AddSeconds(expire);
        var cacheValue = new CacheValue(value, expiration);
        Cache[key] = cacheValue;
    }

    /// <summary>
    /// Internal value wrapper with expiration time.
    /// </summary>
    private record CacheValue(string Value, DateTime Expiration);
}