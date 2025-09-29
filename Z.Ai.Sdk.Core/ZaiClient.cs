using Z.Ai.Sdk.Core.Config;

namespace Z.Ai.Sdk.Core;

/// <summary>
/// Main client for interacting with the Z.ai API. Provides access to all AI services
/// including chat completion, embeddings, file management, audio processing, image generation,
/// video processing, and more.
/// </summary>
public class ZaiClient : AbstractAiClient
{
    /// <summary>
    /// Initializes a new instance of the ZaiClient class with the specified configuration.
    /// </summary>
    /// <param name="config">Z.ai SDK configuration containing API credentials and settings</param>
    /// <param name="baseUrl">Optional base URL for the API endpoint. If not provided, uses the default Z.ai URL</param>
    public ZaiClient(ZaiConfig config)
        : base(config, string.IsNullOrEmpty(config.BaseUrl) ? Constants.ZAiBaseUrl : config.BaseUrl)
    {
    }

    /// <summary>
    /// Creates a new ZaiClient instance configured for the Z.ai platform with the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key in format {apiId}.{apiSecret}</param>
    /// <returns>A new ZaiClient instance configured for Z.ai</returns>
    /// <exception cref="ArgumentException">Thrown when the API key is invalid</exception>
    public static ZaiClient OfZAI(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        var config = new ZaiConfig
        {
            BaseUrl = Constants.ZAiBaseUrl
        }
        .WithApiKey(apiKey);

        return new ZaiClient(config);
    }

    /// <summary>
    /// Creates a new ZaiClient instance configured for the Zhipu AI platform with the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key in format {apiId}.{apiSecret}</param>
    /// <returns>A new ZaiClient instance configured for Zhipu AI</returns>
    /// <exception cref="ArgumentException">Thrown when the API key is invalid</exception>
    public static ZaiClient OfZhipu(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        var config = new ZaiConfig
        {
            BaseUrl = Constants.ZhipuAiBaseUrl
        }
        .WithApiKey(apiKey);

        return new ZaiClient(config);
    }

    /// <summary>
    /// Creates a new ZaiClient instance with custom configuration using object initializer syntax.
    /// </summary>
    /// <returns>A new ZaiClient instance with default configuration that can be customized</returns>
    public static ZaiClient Create() => new(new ZaiConfig());

    /// <summary>
    /// Creates a new ZaiClient instance with the specified API key and default configuration.
    /// </summary>
    /// <param name="apiKey">The API key in format {apiId}.{apiSecret}</param>
    /// <returns>A new ZaiClient instance with the specified API key</returns>
    /// <exception cref="ArgumentException">Thrown when the API key is invalid</exception>
    public static ZaiClient WithApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        var config = new ZaiConfig().WithApiKey(apiKey);
        return new ZaiClient(config);
    }

    /// <summary>
    /// Creates a new ZaiClient instance with custom configuration using object initializer syntax.
    /// This method provides a flexible way to configure the client with various options.
    /// </summary>
    /// <param name="configure">Action to configure the ZaiConfig</param>
    /// <returns>A new ZaiClient instance with the specified configuration</returns>
    public static ZaiClient Configure(Action<ZaiConfig> configure)
    {
        var config = new ZaiConfig();
        configure(config);
        return new ZaiClient(config);
    }
}