using System.Text;
using Z.Ai.Sdk.Core.Cache;
using Z.Ai.Sdk.Core.Config;
using Jose;

namespace Z.Ai.Sdk.Core.Token;

/// <summary>
/// JWT token manager for handling token generation and caching.
/// </summary>
/// <remarks>
/// Constructs TokenManager with specified cache implementation.
/// </remarks>
/// <param name="cache">Cache implementation for token storage</param>
public class TokenManager(ICache cache)
{
    private readonly ICache _cache = cache;
    private const string TokenKeyPrefix = "zai_oapi_token";

    /// <summary>
    /// Additional delay time (5 minutes) to prevent token expiration issues.
    /// </summary>
    private static readonly TimeSpan DelayExpireTime = TimeSpan.FromMinutes(5);

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
    /// <returns>JWT token string</returns>
    public static string CreateJwt(ZaiConfig config)
    {
        if (config.Alg != "HS256")
        {
            throw new NotSupportedException($"Algorithm: {config.Alg} not supported.");
        }

        if (string.IsNullOrEmpty(config.ApiSecret))
            throw new ArgumentException("ApiSecret must be provided.", nameof(config));
        
        // Derive expiration time in ms
        var exp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                + config.ExpireMillis
                + (long)DelayExpireTime.TotalMilliseconds;

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var payload = new Dictionary<string, object>
        {
            { "api_key", config.ApiId },
            { "exp", exp.ToString() },
            { "timestamp", timestamp.ToString() }
        };

        var extraHeaders = new Dictionary<string, object>
        {
            { "sign_type", "SIGN" }
        };

        var secret = Encoding.UTF8.GetBytes(config.ApiSecret);

        return JWT.Encode(payload, secret, JwsAlgorithm.HS256, extraHeaders);
    }

    /// <summary>
    /// Generates cache key for token storage.
    /// </summary>
    /// <param name="apiKey">API key</param>
    /// <returns>Formatted cache key</returns>
    private static string GenerateTokenCacheKey(string apiKey) => $"{TokenKeyPrefix}-{apiKey}";
}