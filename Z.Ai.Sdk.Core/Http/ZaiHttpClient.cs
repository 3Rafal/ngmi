using Z.Ai.Sdk.Core.Config;

namespace Z.Ai.Sdk.Core.Http;

/// <summary>
/// Factory class for creating HttpClient instances configured with Z.AI authentication.
/// </summary>
public static class ZaiHttpClient
{
    /// <summary>
    /// Creates an HttpClient instance configured with Z.AI authentication.
    /// </summary>
    /// <param name="config">Z.AI configuration</param>
    /// <param name="baseAddress">Optional base address for the HttpClient</param>
    /// <returns>A configured HttpClient instance</returns>
    public static HttpClient Create(
        ZaiConfig config,
        Uri? baseAddress = null)
    {
        var handler = new AuthenticationHeaderHandler(config)
        {
            InnerHandler = new HttpClientHandler()
    };

        var httpClient = new HttpClient(handler);

        if (baseAddress != null)
        {
            httpClient.BaseAddress = baseAddress;
        }

        return httpClient;
    }
}