namespace Z.Ai.Sdk.Core.Config;

/// <summary>
/// Configuration class for ZAI SDK containing API credentials, JWT settings, HTTP client
/// configurations, and cache settings. Supports reading configuration values from
/// environment variables with memory values taking priority.
/// </summary>
public record ZaiConfig(
    string? BaseUrl = null,
    string? ApiKey = null,
    string? ApiId = null,
    string? ApiSecret = null,
    IReadOnlyDictionary<string, string>? CustomHeaders = null,
    int ExpireMillis = 30 * 60 * 1000,
    string Alg = "HS256",
    bool DisableTokenCache = false,
    int ConnectionPoolMaxIdleConnections = 5,
    long ConnectionPoolKeepAliveDuration = 1,
    int RequestTimeOut = 300,
    int ConnectTimeout = 100,
    int ReadTimeout = 100,
    int WriteTimeout = 100,
    string SourceChannel = "dotnet-sdk"
)
{
    // Environment variable names
    private const string EnvBaseUrl = "ZAI_BASE_URL";
    private const string EnvApiKey = "ZAI_API_KEY";
    private const string EnvExpireMillis = "ZAI_EXPIRE_MILLIS";
    private const string EnvAlg = "ZAI_ALG";
    private const string EnvDisableTokenCache = "ZAI_DISABLE_TOKEN_CACHE";
    private const string EnvConnectionPoolMaxIdle = "ZAI_CONNECTION_POOL_MAX_IDLE";
    private const string EnvConnectionPoolKeepAlive = "ZAI_CONNECTION_POOL_KEEP_ALIVE";
    private const string EnvRequestTimeout = "ZAI_REQUEST_TIMEOUT";
    private const string EnvConnectTimeout = "ZAI_CONNECT_TIMEOUT";
    private const string EnvReadTimeout = "ZAI_READ_TIMEOUT";
    private const string EnvWriteTimeout = "ZAI_WRITE_TIMEOUT";

    // Default base URL from Z.AI API
    private const string ZaiBaseUrl = "https://api.z.ai/api/paas/v4/";

    /// <summary>
    /// Gets the base URL for API endpoints with environment variable fallback.
    /// </summary>
    public string EffectiveBaseUrl =>
        BaseUrl ??
        Environment.GetEnvironmentVariable(EnvBaseUrl) ??
        ZaiBaseUrl;

    /// <summary>
    /// Gets the combined API key with environment variable fallback.
    /// </summary>
    public string EffectiveApiKey
    {
        get
        {
            if (!string.IsNullOrEmpty(ApiKey))
                return ApiKey;

            var envValue = Environment.GetEnvironmentVariable(EnvApiKey);
            if (!string.IsNullOrEmpty(envValue))
                return envValue;

            return ApiKey ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the API ID component with environment variable fallback.
    /// </summary>
    public string EffectiveApiId
    {
        get
        {
            if (!string.IsNullOrEmpty(ApiId))
                return ApiId;

            var apiKey = EffectiveApiKey;
            if (string.IsNullOrEmpty(apiKey))
                return string.Empty;

            var parts = apiKey.Split('.');
            return parts.Length >= 2 ? parts[0] : string.Empty;
        }
    }

    /// <summary>
    /// Gets the API secret component with environment variable fallback.
    /// </summary>
    public string EffectiveApiSecret
    {
        get
        {
            if (!string.IsNullOrEmpty(ApiSecret))
                return ApiSecret;

            var apiKey = EffectiveApiKey;
            if (string.IsNullOrEmpty(apiKey))
                return string.Empty;

            var parts = apiKey.Split('.');
            return parts.Length >= 2 ? parts[1] : string.Empty;
        }
    }

    /// <summary>
    /// Gets the token expiration time in milliseconds with environment variable fallback.
    /// </summary>
    public int EffectiveExpireMillis
    {
        get
        {
            if (ExpireMillis != 30 * 60 * 1000)
                return ExpireMillis;

            var envValue = Environment.GetEnvironmentVariable(EnvExpireMillis);
            if (int.TryParse(envValue, out var parsedValue))
                return parsedValue;

            return ExpireMillis;
        }
    }

    /// <summary>
    /// Gets the JWT algorithm with environment variable fallback.
    /// </summary>
    public string EffectiveAlg
    {
        get
        {
            if (Alg != "HS256")
                return Alg;

            var envValue = Environment.GetEnvironmentVariable(EnvAlg);
            return envValue ?? Alg;
        }
    }

    /// <summary>
    /// Gets the disable token cache flag with environment variable fallback.
    /// </summary>
    public bool EffectiveDisableTokenCache
    {
        get
        {
            if (DisableTokenCache)
                return true;

            var envValue = Environment.GetEnvironmentVariable(EnvDisableTokenCache);
            return bool.Parse(envValue ?? "false");
        }
    }

    /// <summary>
    /// Gets the connection pool max idle connections with environment variable fallback.
    /// </summary>
    public int EffectiveConnectionPoolMaxIdleConnections
    {
        get
        {
            if (ConnectionPoolMaxIdleConnections != 5)
                return ConnectionPoolMaxIdleConnections;

            var envValue = Environment.GetEnvironmentVariable(EnvConnectionPoolMaxIdle);
            if (int.TryParse(envValue, out var parsedValue))
                return parsedValue;

            return ConnectionPoolMaxIdleConnections;
        }
    }

    /// <summary>
    /// Gets the connection pool keep alive duration with environment variable fallback.
    /// </summary>
    public long EffectiveConnectionPoolKeepAliveDuration
    {
        get
        {
            if (ConnectionPoolKeepAliveDuration != 1)
                return ConnectionPoolKeepAliveDuration;

            var envValue = Environment.GetEnvironmentVariable(EnvConnectionPoolKeepAlive);
            if (long.TryParse(envValue, out var parsedValue))
                return parsedValue;

            return ConnectionPoolKeepAliveDuration;
        }
    }

    /// <summary>
    /// Gets the request timeout with environment variable fallback.
    /// </summary>
    public int EffectiveRequestTimeout
    {
        get
        {
            if (RequestTimeOut != 300)
                return RequestTimeOut;

            var envValue = Environment.GetEnvironmentVariable(EnvRequestTimeout);
            if (int.TryParse(envValue, out var parsedValue))
                return parsedValue;

            return RequestTimeOut;
        }
    }

    /// <summary>
    /// Gets the connect timeout with environment variable fallback.
    /// </summary>
    public int EffectiveConnectTimeout
    {
        get
        {
            if (ConnectTimeout != 100)
                return ConnectTimeout;

            var envValue = Environment.GetEnvironmentVariable(EnvConnectTimeout);
            if (int.TryParse(envValue, out var parsedValue))
                return parsedValue;

            return ConnectTimeout;
        }
    }

    /// <summary>
    /// Gets the read timeout with environment variable fallback.
    /// </summary>
    public int EffectiveReadTimeout
    {
        get
        {
            if (ReadTimeout != 100)
                return ReadTimeout;

            var envValue = Environment.GetEnvironmentVariable(EnvReadTimeout);
            if (int.TryParse(envValue, out var parsedValue))
                return parsedValue;

            return ReadTimeout;
        }
    }

    /// <summary>
    /// Gets the write timeout with environment variable fallback.
    /// </summary>
    public int EffectiveWriteTimeout
    {
        get
        {
            if (WriteTimeout != 100)
                return WriteTimeout;

            var envValue = Environment.GetEnvironmentVariable(EnvWriteTimeout);
            if (int.TryParse(envValue, out var parsedValue))
                return parsedValue;

            return WriteTimeout;
        }
    }

    /// <summary>
    /// Creates a new ZaiConfig with the specified API key.
    /// </summary>
    /// <param name="apiKey">Combined API key in format {apiId}.{apiSecret}</param>
    /// <returns>A new ZaiConfig instance</returns>
    /// <exception cref="ArgumentException">Thrown when the API key format is invalid</exception>
    public static ZaiConfig WithApiKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        var parts = apiKey.Split('.');
        if (parts.Length != 2)
            throw new ArgumentException("API key must be in format {apiId}.{apiSecret}", nameof(apiKey));

        return new ZaiConfig(
            ApiKey: apiKey,
            ApiId: parts[0],
            ApiSecret: parts[1]
        );
    }
}