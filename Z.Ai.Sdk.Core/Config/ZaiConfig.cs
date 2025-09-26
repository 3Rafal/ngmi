namespace Z.Ai.Sdk.Core.Config;

/// <summary>
/// Configuration class for ZAI SDK containing API credentials, JWT settings, HTTP client
/// configurations, and cache settings. Supports reading configuration values from
/// environment variables with memory values taking priority.
/// </summary>
public record ZaiConfig(
    string? baseUrl = null,
    string? apiKey = null,
    string? apiId = null,
    string? apiSecret = null,
    IReadOnlyDictionary<string, string>? customHeaders = null,
    int? expireMillis = null,
    string? alg = null,
    bool? disableTokenCache = null,
    int? connectionPoolMaxIdleConnections = null,
    long? connectionPoolKeepAliveDuration = null,
    int? requestTimeOut = null,
    int? connectTimeout = null,
    int? readTimeout = null,
    int? writeTimeout = null,
    string sourceChannel = "dotnet-sdk"
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
    /// Gets the base URL for API endpoints with environment variable fallback.
    /// </summary>
    public string BaseUrl =>
        baseUrl ??
        Environment.GetEnvironmentVariable(EnvBaseUrl) ??
        Constants.ZAiBaseUrl;

    /// <summary>
    /// Gets the combined API key with environment variable fallback.
    /// </summary>
    public string ApiKey =>
        !string.IsNullOrEmpty(apiKey)
            ? apiKey
            : Environment.GetEnvironmentVariable(EnvApiKey) ?? string.Empty;

    /// <summary>
    /// Gets the API ID component with environment variable fallback.
    /// </summary>
    public string ApiId =>
        !string.IsNullOrEmpty(apiId) ? apiId :
        !string.IsNullOrEmpty(ApiKey) && ApiKey.Contains('.') ? ApiKey.Split('.')[0] :
        string.Empty;

    /// <summary>
    /// Gets the API secret component with environment variable fallback.
    /// </summary>
    public string ApiSecret =>
        !string.IsNullOrEmpty(apiSecret) ? apiSecret :
        !string.IsNullOrEmpty(ApiKey) && ApiKey.Contains('.') ? ApiKey.Split('.')[1] :
        string.Empty;

    /// <summary>
    /// Gets custom HTTP request headers.
    /// </summary>
    public IReadOnlyDictionary<string, string>? CustomHeaders => customHeaders;

    /// <summary>
    /// Gets the token expiration time in milliseconds with environment variable fallback.
    /// </summary>
    public int ExpireMillis =>
        expireMillis ??
        (int.TryParse(Environment.GetEnvironmentVariable(EnvExpireMillis), out var envExpire) ? envExpire : DefaultExpireMillis);

    /// <summary>
    /// Gets the JWT algorithm with environment variable fallback.
    /// </summary>
    public string Alg =>
        alg ??
        Environment.GetEnvironmentVariable(EnvAlg) ?? DefaultAlg;

    /// <summary>
    /// Gets the disable token cache flag with environment variable fallback.
    /// </summary>
    public bool DisableTokenCache =>
        disableTokenCache ??
        (bool.TryParse(Environment.GetEnvironmentVariable(EnvDisableTokenCache), out var envDisable) ? envDisable : false);

    /// <summary>
    /// Gets the connection pool max idle connections with environment variable fallback.
    /// </summary>
    public int ConnectionPoolMaxIdleConnections =>
        connectionPoolMaxIdleConnections ??
        (int.TryParse(Environment.GetEnvironmentVariable(EnvConnectionPoolMaxIdle), out var envMaxIdle) ? envMaxIdle : DefaultConnectionPoolMaxIdle);

    /// <summary>
    /// Gets the connection pool keep alive duration with environment variable fallback.
    /// </summary>
    public long ConnectionPoolKeepAliveDuration =>
        connectionPoolKeepAliveDuration ??
        (long.TryParse(Environment.GetEnvironmentVariable(EnvConnectionPoolKeepAlive), out var envKeepAlive) ? envKeepAlive : DefaultConnectionPoolKeepAlive);

    /// <summary>
    /// Gets the request timeout with environment variable fallback.
    /// </summary>
    public int RequestTimeOut =>
        requestTimeOut ??
        (int.TryParse(Environment.GetEnvironmentVariable(EnvRequestTimeout), out var envRequestTimeout) ? envRequestTimeout : DefaultRequestTimeout);

    /// <summary>
    /// Gets the connect timeout with environment variable fallback.
    /// </summary>
    public int ConnectTimeout =>
        connectTimeout ??
        (int.TryParse(Environment.GetEnvironmentVariable(EnvConnectTimeout), out var envConnectTimeout) ? envConnectTimeout : DefaultConnectTimeout);

    /// <summary>
    /// Gets the read timeout with environment variable fallback.
    /// </summary>
    public int ReadTimeout =>
        readTimeout ??
        (int.TryParse(Environment.GetEnvironmentVariable(EnvReadTimeout), out var envReadTimeout) ? envReadTimeout : DefaultReadTimeout);

    /// <summary>
    /// Gets the write timeout with environment variable fallback.
    /// </summary>
    public int WriteTimeout =>
        writeTimeout ??
        (int.TryParse(Environment.GetEnvironmentVariable(EnvWriteTimeout), out var envWriteTimeout) ? envWriteTimeout : DefaultWriteTimeout);

    /// <summary>
    /// Gets the source channel identifier for request tracking.
    /// </summary>
    public string SourceChannel => sourceChannel;

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
            apiKey: apiKey,
            apiId: parts[0],
            apiSecret: parts[1]
        );
    }
}