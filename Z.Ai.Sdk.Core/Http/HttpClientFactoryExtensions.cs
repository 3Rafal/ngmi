using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Z.Ai.Sdk.Core.Config;

namespace Z.Ai.Sdk.Core.Http;

/// <summary>
/// Extension methods for configuring HttpClient with authentication handlers.
/// </summary>
public static class HttpClientFactoryExtensions
{
    /// <summary>
    /// Adds an HttpClient configured with Z.AI authentication to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="config">Z.AI configuration</param>
    /// <param name="clientName">Optional name for the HttpClient</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddZAiHttpClient(
        this IServiceCollection services,
        ZaiConfig config,
        string? clientName = null)
    {
        // Register the authentication handler
        services.AddTransient<AuthenticationHeaderHandler>();

        // Configure HttpClient with the authentication handler
        if (string.IsNullOrEmpty(clientName))
        {
            services.AddHttpClient("Default")
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();
        }
        else
        {
            services.AddHttpClient(clientName)
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();
        }

        return services;
    }

    /// <summary>
    /// Creates an HttpClient instance configured with Z.AI authentication.
    /// </summary>
    /// <param name="config">Z.AI configuration</param>
    /// <param name="baseAddress">Optional base address for the HttpClient</param>
    /// <returns>A configured HttpClient instance</returns>
    public static HttpClient CreateZAiHttpClient(
        ZaiConfig config,
        Uri? baseAddress = null)
    {
        var handler = new AuthenticationHeaderHandler(config);
        var httpClient = new HttpClient(handler);

        if (baseAddress != null)
        {
            httpClient.BaseAddress = baseAddress;
        }

        return httpClient;
    }
}