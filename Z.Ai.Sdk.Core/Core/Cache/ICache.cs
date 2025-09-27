namespace Z.Ai.Sdk.Core.Cache;

/// <summary>
/// Token cache interface with default LocalCache implementation. Can be replaced with
/// distributed cache (e.g., Redis) as needed.
/// </summary>
public interface ICache
{
    /// <summary>
    /// Retrieves cached value by key.
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>Cached value or empty string if not found or expired</returns>
    string Get(string key);

    /// <summary>
    /// Sets cache value with expiration time.
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <param name="value">The value to cache</param>
    /// <param name="expire">Expiration duration in seconds</param>
    void Set(string key, string value, int expire);
}