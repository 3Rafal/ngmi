using System.Text.Json;
using Z.Ai.Sdk.Core.Config;
using Z.Ai.Sdk.Core.Http;
using Z.Ai.Sdk.Core.Model;
using Z.Ai.Sdk.Core.Service;
using Z.Ai.Sdk.Core.Service.Model;
using Z.Ai.Sdk.Core.Service.Agents;
using Z.Ai.Sdk.Core.Service.Assistant;
using Z.Ai.Sdk.Core.Service.Audio;
using Z.Ai.Sdk.Core.Service.Batches;
using Z.Ai.Sdk.Core.Service.Chat;
using Z.Ai.Sdk.Core.Service.Embedding;
using Z.Ai.Sdk.Core.Service.File;
using Z.Ai.Sdk.Core.Service.Images;
using Z.Ai.Sdk.Core.Service.Videos;
using Z.Ai.Sdk.Core.Service.VoiceClone;
using Z.Ai.Sdk.Core.Service.WebSearch;
using Microsoft.Extensions.Logging;

namespace Z.Ai.Sdk.Core;

/// <summary>
/// Abstract base class for AI clients providing HTTP client management,
/// service lazy initialization, and request execution capabilities.
/// </summary>
public abstract class AbstractAiClient : AbstractClientBaseService, IDisposable
{
    private readonly ILogger _logger = ZaiLogger.GetLogger<AbstractAiClient>();
    private readonly ZaiConfig _config;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly string _baseUrl;

    // Service instances - lazily initialized for thread safety and performance
    private IAgentService? _agentService;
    private IAssistantService? _assistantService;
    private IAudioService? _audioService;
    private IBatchesService? _batchesService;
    private IChatService? _chatService;
    private IEmbeddingService? _embeddingService;
    private IFileService? _fileService;
    private IImageService? _imageService;
    private IVideoService? _videoService;
    private IVoiceCloneService? _voiceCloneService;
    private IWebSearchService? _webSearchService;


    /// <summary>
    /// Initializes a new instance of the AbstractAiClient class.
    /// </summary>
    /// <param name="config">Z.AI SDK configuration</param>
    /// <param name="baseUrl">Base URL for the API endpoint</param>
    protected AbstractAiClient(ZaiConfig config, string baseUrl)
    {
        _logger.LogInformation("ZAI Init the client: {ClassName}, baseUrl: {BaseUrl}", GetType().Name, baseUrl);
        _config = config;
        _baseUrl = baseUrl;
        _httpClient = CreateHttpClient(config);
        _jsonSerializerOptions = CreateJsonSerializerOptions();
    }

    /// <summary>
    /// Gets the Z.AI SDK configuration.
    /// </summary>
    protected ZaiConfig Config => _config;

    /// <summary>
    /// Gets the HTTP client instance.
    /// </summary>
    protected HttpClient HttpClient => _httpClient;

    /// <summary>
    /// Gets the JSON serializer options.
    /// </summary>
    protected JsonSerializerOptions JsonSerializerOptions => _jsonSerializerOptions;

    // Service accessors - using synchronized lazy initialization like Java version
    /// <summary>
    /// Gets the agent service for AI agent management.
    /// </summary>
    public IAgentService Agents() => GetOrCreateService(ref _agentService, () => new AgentService(this));

    /// <summary>
    /// Gets the assistant service for AI assistant management.
    /// </summary>
    public IAssistantService Assistants() => GetOrCreateService(ref _assistantService, () => new AssistantService(this));

    /// <summary>
    /// Gets the audio service for audio processing.
    /// </summary>
    public IAudioService Audio() => GetOrCreateService(ref _audioService, () => new AudioService(this));

    /// <summary>
    /// Gets the batch service for batch processing.
    /// </summary>
    public IBatchesService Batches() => GetOrCreateService(ref _batchesService, () => new BatchesService(this));

    /// <summary>
    /// Gets the chat service for conversational AI.
    /// </summary>
    public IChatService Chat() => GetOrCreateService(ref _chatService, () => new ChatService(this));

    /// <summary>
    /// Gets the embedding service for text embeddings.
    /// </summary>
    public IEmbeddingService Embeddings() => GetOrCreateService(ref _embeddingService, () => new EmbeddingService(this));

    /// <summary>
    /// Gets the file service for file management.
    /// </summary>
    public IFileService Files() => GetOrCreateService(ref _fileService, () => new FileService(this));

    /// <summary>
    /// Gets the image service for image processing.
    /// </summary>
    public IImageService GetImages() => GetOrCreateService(ref _imageService, () => new ImageService(this));

    /// <summary>
    /// Gets the videos service for video processing.
    /// </summary>
    public IVideoService Videos() => GetOrCreateService(ref _videoService, () => new VideoService(this));

    /// <summary>
    /// Gets the voice clone service for voice cloning.
    /// </summary>
    public IVoiceCloneService VoiceClone() => GetOrCreateService(ref _voiceCloneService, () => new VoiceCloneService(this));

    /// <summary>
    /// Gets the web search service for internet search.
    /// </summary>
    public IWebSearchService WebSearch() => GetOrCreateService(ref _webSearchService, () => new WebSearchService(this));

    // Helper method for thread-safe service creation
    private static T GetOrCreateService<T>(ref T? serviceField, Func<T> factory) where T : class
    {
        if (serviceField == null)
        {
            lock (typeof(T))
            {
                if (serviceField == null)
                {
                    serviceField = factory();
                }
            }
        }
        return serviceField;
    }

    // ==================== Utility Methods ====================

    /// <summary>
    /// Returns the underlying HTTP client instance used for API communication. This method is
    /// primarily intended for advanced users who need direct access to the HTTP client
    /// for custom API calls.
    /// </summary>
    /// <returns>The HTTP client instance</returns>
    public HttpClient GetHttpClient()
    {
        return _httpClient;
    }

    /// <summary>
    /// Creates a Refit service instance for the specified API interface type. This method is
    /// primarily intended for internal use by service implementations and advanced users who need
    /// direct access to Refit-generated API clients.
    /// </summary>
    /// <typeparam name="T">The API interface type</typeparam>
    /// <returns>A configured Refit service instance</returns>
    public T CreateRefitService<T>() where T : class
    {
        return RefitServiceFactory.Create<T>(_config, _baseUrl);
    }

    /// <summary>
    /// Creates a Refit service instance for streaming operations. This method is optimized for
    /// Server-Sent Events (SSE) and other streaming API operations.
    /// </summary>
    /// <typeparam name="T">The API interface type</typeparam>
    /// <returns>A configured Refit service instance for streaming</returns>
    public T CreateRefitStreamingService<T>() where T : class
    {
        return RefitServiceFactory.CreateForStreaming<T>(_config, _baseUrl);
    }

    // ==================== Core Request Execution Methods ====================

    /// <summary>
    /// Executes a synchronous API request and returns the response. This method handles
    /// the complete request lifecycle including error handling and response wrapping.
    /// </summary>
    /// <typeparam name="TData">The type of data expected in the response</typeparam>
    /// <typeparam name="TParam">The type of parameters for the request</typeparam>
    /// <typeparam name="TRequest">The type of client request</typeparam>
    /// <typeparam name="TResponse">The type of client response</typeparam>
    /// <param name="request">The client request containing parameters</param>
    /// <param name="requestSupplier">The supplier that creates the actual API call</param>
    /// <param name="responseType">The type of the response</param>
    /// <returns>The wrapped response containing either success data or error information</returns>
    public override async Task<TResponse> ExecuteRequest<TData, TParam, TRequest, TResponse>(
        TRequest request,
        Func<TParam, Task<TData>> requestSupplier,
        Type responseType)
    {
        var apiCall = requestSupplier((TParam)(object)request);

        var response = CreateClientResponse<TData, TResponse>(responseType);

        try
        {
            // Execute the API call
            var data = await Execute(() => apiCall);

            response.Data = data;
            response.Code = 200;
            response.Msg = "Call Successful";
            response.Success = true;
        }
        catch (ZaiHttpException ex)
        {
            _logger.LogError("API request failed with call error: {Exception}", ex);
            response.Code = ex.StatusCode ?? 500;
            response.Msg = "Call Failed";
            response.Success = false;
            response.Error = new ChatError
            {
                Code = !string.IsNullOrEmpty(ex.Code) ? int.Parse(ex.Code) : null,
                Message = ex.Message
            };
        }

        return response;
    }

    /// <summary>
    /// Executes a streaming API request and returns a response with different response and stream types.
    /// </summary>
    /// <typeparam name="TData">Type of response body</typeparam>
    /// <typeparam name="TStream">Type of each element in the stream</typeparam>
    /// <typeparam name="TParam">Request parameter type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="request">The request to send</param>
    /// <param name="requestSupplier">Factory that creates the API call</param>
    /// <param name="responseType">The response class type</param>
    /// <param name="streamItemType">The class of the stream element</param>
    /// <returns>Streaming client response</returns>
    public override async Task<TResponse> BiStreamRequest<TData, TStream, TParam, TRequest, TResponse>(
        TRequest request,
        Func<TParam, Task<Stream>> requestSupplier,
        Type responseType,
        Type streamItemType)
    {
        var apiCall = requestSupplier((TParam)(object)request);
        var response = CreateClientResponse<TData, TResponse>(responseType);

        try
        {
            var stream = await Execute(() => apiCall);
            var streamItems = ParseSseStream<TStream>(stream, streamItemType);

            response.Stream = streamItems;
            response.Code = 200;
            response.Msg = "Stream initialized successfully";
            response.Success = true;
        }
        catch (ZaiHttpException ex)
        {
            HandleStreamError(response, ex);
        }

        return response;
    }

    /// <summary>
    /// Handles stream errors by setting error properties on the response.
    /// </summary>
    /// <param name="response">The response to update with error information</param>
    /// <param name="ex">The exception that occurred</param>
    private void HandleStreamError<TData>(IClientResponse<TData> response, ZaiHttpException ex)
    {
        _logger.LogError("Streaming API request failed with business error: {Exception}", ex);
        response.Code = ex.StatusCode ?? 500;
        response.Msg = "Business error";
        response.Success = false;
        response.Error = new ChatError
        {
            Code = !string.IsNullOrEmpty(ex.Code) ? int.Parse(ex.Code) : null,
            Message = ex.Message
        };
    }

    /// <summary>
    /// Creates a new instance of the specified response class using reflection. This
    /// helper method is used to instantiate response objects dynamically.
    /// </summary>
    /// <typeparam name="TData">The type of data contained in the response</typeparam>
    /// <typeparam name="TResponse">The type of client response</typeparam>
    /// <param name="responseType">The type of the response to instantiate</param>
    /// <returns>A new instance of the response class</returns>
    /// <exception cref="InvalidOperationException">If the response object cannot be created</exception>
    private static TResponse CreateClientResponse<TData, TResponse>(Type responseType)
        where TResponse : IClientResponse<TData>, new()
    {
        try
        {
            return (TResponse)Activator.CreateInstance(responseType)!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create response object of type: {responseType.Name}", ex);
        }
    }

    /// <summary>
    /// Parses Server-Sent Events (SSE) stream and returns an async enumerable of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize each SSE event to</typeparam>
    /// <param name="stream">The input stream containing SSE data</param>
    /// <param name="itemType">The type of each stream item</param>
    /// <returns>An async enumerable of deserialized items</returns>
    private static async IAsyncEnumerable<T> ParseSseStream<T>(Stream stream, Type itemType)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(line))
                continue;

            if (line.StartsWith("data: "))
            {
                var data = line["data: ".Length..];
                if (data != "[DONE]")
                {
                    var item = JsonSerializer.Deserialize(data, itemType, options);
                    if (item != null)
                    {
                        yield return (T)item;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Closes the AI client and releases all associated resources.
    /// </summary>
    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    // Helper methods for base class construction
    private static HttpClient CreateHttpClient(ZaiConfig config)
    {
        var baseUri = new Uri(config.BaseUrl);
        return ZaiHttpClient.Create(config, baseUri);
    }

    private static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Abstract base builder class for creating AI client instances with custom configurations.
    /// </summary>
    public abstract class AbstractBuilder<TClient> where TClient : AbstractAiClient
    {
        protected ZaiConfig Config { get; private set; } = new ZaiConfig();

        /// <summary>
        /// Initializes a new instance of the AbstractBuilder class.
        /// </summary>
        protected AbstractBuilder() { }

        /// <summary>
        /// Initializes a new instance of the AbstractBuilder class with the specified API key.
        /// </summary>
        /// <param name="apiKey">The API key for authentication</param>
        protected AbstractBuilder(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

            Config.ApiKey = apiKey;
        }

        /// <summary>
        /// Initializes a new instance of the AbstractBuilder class with custom base URL and API key.
        /// </summary>
        /// <param name="baseUrl">The custom base URL for the API endpoint</param>
        /// <param name="apiKey">The API secret key for authentication</param>
        protected AbstractBuilder(string baseUrl, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty", nameof(baseUrl));

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

            Config.BaseUrl = baseUrl;
            Config.ApiKey = apiKey;
        }

        /// <summary>
        /// Sets the base URL for the API service.
        /// </summary>
        /// <param name="baseUrl">Base URL</param>
        /// <returns>This builder instance for method chaining</returns>
        public AbstractBuilder<TClient> BaseUrl(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty", nameof(baseUrl));

            Config.BaseUrl = baseUrl;
            return this;
        }

        /// <summary>
        /// Sets the API key for authentication.
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <returns>This builder instance for method chaining</returns>
        public AbstractBuilder<TClient> ApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

            Config.ApiKey = apiKey;
            return this;
        }

        /// <summary>
        /// Sets custom headers for HTTP requests.
        /// </summary>
        /// <param name="customHeaders">Custom headers dictionary</param>
        /// <returns>This builder instance for method chaining</returns>
        public AbstractBuilder<TClient> CustomHeaders(Dictionary<string, string> customHeaders)
        {
            if (customHeaders == null || customHeaders.Count == 0)
                throw new ArgumentException("Custom headers cannot be null or empty", nameof(customHeaders));

            Config.CustomHeaders = customHeaders;
            return this;
        }

        /// <summary>
        /// Disables token caching, forcing the client to use API keys for direct requests.
        /// </summary>
        /// <returns>This builder instance for method chaining</returns>
        public AbstractBuilder<TClient> DisableTokenCache()
        {
            Config.DisableTokenCache = true;
            return this;
        }

        /// <summary>
        /// Enables token caching, allowing the client to use access tokens for requests.
        /// </summary>
        /// <returns>This builder instance for method chaining</returns>
        public AbstractBuilder<TClient> EnableTokenCache()
        {
            Config.DisableTokenCache = false;
            return this;
        }

        /// <summary>
        /// Sets the token expiration time in milliseconds.
        /// </summary>
        /// <param name="expireMillis">The token expiration time in milliseconds</param>
        /// <returns>This builder instance for method chaining</returns>
        public AbstractBuilder<TClient> TokenExpire(int expireMillis)
        {
            Config.ExpireMillis = expireMillis;
            return this;
        }

        /// <summary>
        /// Configures network request timeout settings.
        /// </summary>
        /// <param name="requestTimeout">The overall request timeout in seconds</param>
        /// <param name="connectTimeout">The connection timeout in seconds</param>
        /// <param name="readTimeout">The read timeout in seconds</param>
        /// <param name="writeTimeout">The write timeout in seconds</param>
        /// <returns>This builder instance for method chaining</returns>
        public AbstractBuilder<TClient> NetworkConfig(int requestTimeout, int connectTimeout, int readTimeout, int writeTimeout)
        {
            Config.RequestTimeOut = requestTimeout;
            Config.ConnectTimeout = connectTimeout;
            Config.ReadTimeout = readTimeout;
            Config.WriteTimeout = writeTimeout;
            return this;
        }

        /// <summary>
        /// Builds and returns a new AI client instance with the configured settings.
        /// </summary>
        /// <returns>A new AI client instance</returns>
        public abstract TClient Build();
    }
}