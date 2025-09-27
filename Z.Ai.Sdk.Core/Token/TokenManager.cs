using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Z.Ai.Sdk.Core.Cache;
using Z.Ai.Sdk.Core.Config;

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

        // Create symmetric key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.ApiSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Add payload claims
        var claims = new List<Claim>
        {
            new("api_key", config.ApiId),
            new("exp", (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + config.ExpireMillis + (long)DelayExpireTime.TotalMilliseconds).ToString()),
            new("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString())
        };

        var token = new JwtSecurityToken(
            header: new JwtHeader(creds) { {"sign_type", "SIGN"} },
            payload: new JwtPayload(claims)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates cache key for token storage.
    /// </summary>
    /// <param name="apiKey">API key</param>
    /// <returns>Formatted cache key</returns>
    private static string GenerateTokenCacheKey(string apiKey) => $"{TokenKeyPrefix}-{apiKey}";
}