namespace Z.Ai.Sdk.Core.Config;

/// <summary>
/// Configuration class for ZAI SDK containing API credentials, JWT settings, HTTP client
/// configurations, and cache settings. Supports reading configuration values from
/// environment variables with memory values taking priority.
/// </summary>
public class ZaiConfig
{
    // Backing fields
    private string? _baseUrl;
    private string? _apiKey;
    private string? _apiId;
    private string? _apiSecret;
    private IReadOnlyDictionary<string, string>? _customHeaders;
    private int? _expireMillis;
    private string? _alg;
    private bool? _disableTokenCache;
    private int? _connectionPoolMaxIdleConnections;
    private long? _connectionPoolKeepAliveDuration;
    private int? _requestTimeOut;
    private int? _connectTimeout;
    private int? _readTimeout;
    private int? _writeTimeout;
    private string _sourceChannel = "dotnet-sdk";

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

    // Default values
    private const int DefaultExpireMillis = 30 * 60 * 1000;
    private const string DefaultAlg = "HS256";
    private const int DefaultConnectionPoolMaxIdle = 5;
    private const long DefaultConnectionPoolKeepAlive = 1;
    private const int DefaultRequestTimeout = 300;
    private const int DefaultConnectTimeout = 100;
    private const int DefaultReadTimeout = 100;
    private const int DefaultWriteTimeout = 100;

    /// <summary>
    /// Gets or sets the base URL for API endpoints with environment variable fallback.
    /// </summary>
    public string BaseUrl
    {
        get => _baseUrl
            ?? Environment.GetEnvironmentVariable(EnvBaseUrl)
            ?? Constants.ZAiBaseUrl;
        set => _baseUrl = value;
    }

    /// <summary>
    /// Gets or sets the combined API key with environment variable fallback.
    /// </summary>
    public string ApiKey
    {
        get => _apiKey
            ?? Environment.GetEnvironmentVariable(EnvApiKey)
            ?? string.Empty;
        set => _apiKey = value;
    }

    /// <summary>
    /// Gets or sets the API ID component with environment variable fallback.
    /// </summary>
    public string ApiId
    {
        get => !string.IsNullOrEmpty(_apiId) ? _apiId :
            !string.IsNullOrEmpty(ApiKey) && ApiKey.Contains('.') ? ApiKey.Split('.')[0] :
            string.Empty;
        private set => _apiId = value;
    }

    /// <summary>
    /// Gets or sets the API secret component with environment variable fallback.
    /// </summary>
    public string ApiSecret
    {
        get => !string.IsNullOrEmpty(_apiSecret) ? _apiSecret :
            !string.IsNullOrEmpty(ApiKey) && ApiKey.Contains('.') ? ApiKey.Split('.')[1] :
            string.Empty;
        set => _apiSecret = value;
    }

    /// <summary>
    /// Gets or sets custom HTTP request headers.
    /// </summary>
    public IReadOnlyDictionary<string, string>? CustomHeaders
    {
        get => _customHeaders;
        set => _customHeaders = value;
    }

    /// <summary>
    /// Gets or sets the token expiration time in milliseconds with environment variable fallback.
    /// </summary>
    public int ExpireMillis
    {
        get => _expireMillis ??
            (int.TryParse(Environment.GetEnvironmentVariable(EnvExpireMillis), out var envExpire) ? envExpire : DefaultExpireMillis);
        set => _expireMillis = value;
    }

    /// <summary>
    /// Gets or sets the JWT algorithm with environment variable fallback.
    /// </summary>
    public string Alg
    {
        get => _alg ?? Environment.GetEnvironmentVariable(EnvAlg) ?? DefaultAlg;
        set => _alg = value;
    }

    /// <summary>
    /// Gets or sets the disable token cache flag with environment variable fallback.
    /// </summary>
    public bool DisableTokenCache
    {
        get => _disableTokenCache ??
            (bool.TryParse(Environment.GetEnvironmentVariable(EnvDisableTokenCache), out var envDisable) ? envDisable : false);
        set => _disableTokenCache = value;
    }

    /// <summary>
    /// Gets or sets the connection pool max idle connections with environment variable fallback.
    /// </summary>
    public int ConnectionPoolMaxIdleConnections
    {
        get => _connectionPoolMaxIdleConnections ??
            (int.TryParse(Environment.GetEnvironmentVariable(EnvConnectionPoolMaxIdle), out var envMaxIdle) ? envMaxIdle : DefaultConnectionPoolMaxIdle);
        set => _connectionPoolMaxIdleConnections = value;
    }

    /// <summary>
    /// Gets or sets the connection pool keep alive duration with environment variable fallback.
    /// </summary>
    public long ConnectionPoolKeepAliveDuration
    {
        get => _connectionPoolKeepAliveDuration ??
            (long.TryParse(Environment.GetEnvironmentVariable(EnvConnectionPoolKeepAlive), out var envKeepAlive) ? envKeepAlive : DefaultConnectionPoolKeepAlive);
        set => _connectionPoolKeepAliveDuration = value;
    }

    /// <summary>
    /// Gets or sets the request timeout with environment variable fallback.
    /// </summary>
    public int RequestTimeOut
    {
        get => _requestTimeOut ??
            (int.TryParse(Environment.GetEnvironmentVariable(EnvRequestTimeout), out var envRequestTimeout) ? envRequestTimeout : DefaultRequestTimeout);
        set => _requestTimeOut = value;
    }

    /// <summary>
    /// Gets or sets the connect timeout with environment variable fallback.
    /// </summary>
    public int ConnectTimeout
    {
        get => _connectTimeout ??
            (int.TryParse(Environment.GetEnvironmentVariable(EnvConnectTimeout), out var envConnectTimeout) ? envConnectTimeout : DefaultConnectTimeout);
        set => _connectTimeout = value;
    }

    /// <summary>
    /// Gets or sets the read timeout with environment variable fallback.
    /// </summary>
    public int ReadTimeout
    {
        get => _readTimeout ??
            (int.TryParse(Environment.GetEnvironmentVariable(EnvReadTimeout), out var envReadTimeout) ? envReadTimeout : DefaultReadTimeout);
        set => _readTimeout = value;
    }

    /// <summary>
    /// Gets or sets the write timeout with environment variable fallback.
    /// </summary>
    public int WriteTimeout
    {
        get => _writeTimeout ??
            (int.TryParse(Environment.GetEnvironmentVariable(EnvWriteTimeout), out var envWriteTimeout) ? envWriteTimeout : DefaultWriteTimeout);
        set => _writeTimeout = value;
    }

    /// <summary>
    /// Gets or sets the source channel identifier for request tracking.
    /// </summary>
    public string SourceChannel
    {
        get => _sourceChannel;
        set => _sourceChannel = value;
    }

    /// <summary>
    /// Creates a new ZaiConfig with the specified API key.
    /// </summary>
    /// <param name="apiKey">Combined API key in format {apiId}.{apiSecret}</param>
    /// <returns>A new ZaiConfig instance</returns>
    /// <exception cref="ArgumentException">Thrown when the API key format is invalid</exception>
    public ZaiConfig WithApiKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        var parts = apiKey.Split('.');
        if (parts.Length != 2)
            throw new ArgumentException("API key must be in format {apiId}.{apiSecret}", nameof(apiKey));

        ApiKey = apiKey;
        ApiId = parts[0];
        ApiSecret = parts[1];

        return this;

    }
}