using System.IO;
using System.Net.Http.Headers;
using Z.Ai.Sdk.Core.Config;
using Z.Ai.Sdk.Core.Token;

namespace Z.Ai.Sdk.Core.Http;

/// <summary>
/// HTTP message handler that adds authorization and custom headers to outgoing requests.
/// </summary>
public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly ZaiConfig _config;

    /// <summary>
    /// Initializes a new instance of the AuthenticationHeaderHandler.
    /// </summary>
    /// <param name="config">Z.AI configuration containing API credentials</param>
    /// <param name="tokenManager">Optional token manager for JWT token handling</param>
    /// <exception cref="ArgumentNullException">Thrown when config is null</exception>
    /// <exception cref="ArgumentException">Thrown when ApiKey is null or empty</exception>
    public AuthenticationHeaderHandler(ZaiConfig config)
    {
        _config = config;

        if (string.IsNullOrEmpty(config.ApiKey))
        {
            throw new ArgumentException("Z.ai API key is required", nameof(config));
        }
    }

    /// <summary>
    /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation,
    /// adding authentication and custom headers.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = GetAccessToken();

        // Clone the original request to avoid modifying it
        var authenticatedRequest = await CloneHttpRequestMessageAsync(request, cancellationToken);

        // Add standard headers
        authenticatedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        authenticatedRequest.Headers.Add("x-source-channel", _config.SourceChannel);
        authenticatedRequest.Headers.Add("Zai-SDK-Ver", "0.0.5");
        authenticatedRequest.Headers.Add("Accept-Language", "en-US,en");

        // Add custom headers if configured
        if (_config.CustomHeaders != null)
        {
            foreach (var header in _config.CustomHeaders)
            {
                authenticatedRequest.Headers.Add(header.Key, header.Value);
            }
        }

        return await base.SendAsync(authenticatedRequest, cancellationToken);
    }

    /// <summary>
    /// Gets the access token, either from the token manager or directly from config.
    /// </summary>
    /// <returns>The access token string</returns>
    private string GetAccessToken()
    {
        if (_config.DisableTokenCache)
        {
            return _config.ApiKey;
        }

        return GlobalTokenManager.GetTokenManagerV4().GetToken(_config);
    }

    /// <summary>
    /// Clones an HTTP request message to avoid modifying the original.
    /// </summary>
    /// <param name="request">The original request to clone</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A cloned HTTP request message</returns>
    private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);

        // Copy content
        if (request.Content != null)
        {
            var memoryStream = new MemoryStream();
            await request.Content.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;
            clone.Content = new StreamContent(memoryStream);

            // Copy content headers
            foreach (var header in request.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Copy headers
        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }
}