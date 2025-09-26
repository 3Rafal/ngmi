using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Z.Ai.Sdk.Core.Cache;
using Z.Ai.Sdk.Core.Config;

namespace Z.Ai.Sdk.Core.Token;

/// <summary>
/// JWT token manager for handling token generation and caching.
/// </summary>
public class TokenManager
{
    private readonly ICache _cache;
    private const string TokenKeyPrefix = "zai_oapi_token";

    /// <summary>
    /// Additional delay time (5 minutes) to prevent token expiration issues.
    /// </summary>
    private static readonly TimeSpan DelayExpireTime = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Constructs TokenManager with specified cache implementation.
    /// </summary>
    /// <param name="cache">Cache implementation for token storage</param>
    public TokenManager(ICache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <summary>
    /// Gets valid JWT token, either from cache or generates new one.
    /// </summary>
    /// <param name="config">Z.AI configuration containing API credentials</param>
    /// <returns>Valid JWT token</returns>
    public string GetToken(ZaiConfig config)
    {
        var tokenCacheKey = GenerateTokenCacheKey(config.ApiId);
        var cachedToken = _cache.Get(tokenCacheKey);

        if (!string.IsNullOrEmpty(cachedToken))
        {
            return cachedToken;
        }

        var newToken = CreateJwt(config);
        var expireSeconds = config.ExpireMillis / 1000;
        _cache.Set(tokenCacheKey, newToken, expireSeconds);

        return newToken;
    }

    /// <summary>
    /// Creates JWT token using HMAC256 algorithm.
    /// </summary>
    /// <param name="config">Z.AI configuration</param>
    /// <returns>JWT token string or null if creation fails</returns>
    private static string CreateJwt(ZaiConfig config)
    {
        if (config.Alg != "HS256")
        {
            throw new NotSupportedException($"Algorithm {config.Alg} is not supported. Only HS256 is supported.");
        }

        try
        {
            var key = Encoding.UTF8.GetBytes(config.ApiSecret);

            // Create header
            var header = new Dictionary<string, object>
            {
                { "alg", "HS256" },
                { "sign_type", "SIGN" }
            };

            // Create payload
            var payload = new Dictionary<string, object>
            {
                { "api_key", config.ApiId },
                { "exp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + config.ExpireMillis + (long)DelayExpireTime.TotalMilliseconds },
                { "timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() }
            };

            // Create JWT token
            var headerEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(header)));
            var payloadEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));
            var message = $"{headerEncoded}.{payloadEncoded}";

            using var hmac = new HMACSHA256(key);
            var signature = Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(message)));

            return $"{message}.{signature}";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create JWT token", ex);
        }
    }

    /// <summary>
    /// Generates cache key for token storage.
    /// </summary>
    /// <param name="apiKey">API key</param>
    /// <returns>Formatted cache key</returns>
    private static string GenerateTokenCacheKey(string apiKey)
    {
        return $"{TokenKeyPrefix}-{apiKey}";
    }

    /// <summary>
    /// Base64 URL encodes the specified byte array.
    /// </summary>
    /// <param name="input">The byte array to encode</param>
    /// <returns>Base64 URL encoded string</returns>
    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}