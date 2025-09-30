using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Config;
using Z.Ai.Sdk.Core.Service.Assistant.Message;
using Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

namespace Z.Ai.Sdk.Core.Http;

/// <summary>
/// Factory class for creating Refit instances configured with Z.AI authentication and settings.
/// </summary>
public static class RefitServiceFactory
{
    /// <summary>
    /// Creates a Refit RestService instance configured with Z.AI authentication.
    /// </summary>
    /// <typeparam name="T">The API interface type</typeparam>
    /// <param name="config">Z.AI configuration</param>
    /// <param name="baseUrl">Base URL for the API endpoint</param>
    /// <returns>A configured Refit service instance</returns>
    public static T Create<T>(ZaiConfig config, string baseUrl) where T : class
    {
        var httpClient = ZaiHttpClient.Create(config, new Uri(baseUrl));

        return RestService.For<T>(httpClient, GetRefitSettings());
    }

    /// <summary>
    /// Creates a Refit RestService instance with existing HttpClient.
    /// </summary>
    /// <typeparam name="T">The API interface type</typeparam>
    /// <param name="httpClient">Configured HttpClient instance</param>
    /// <returns>A configured Refit service instance</returns>
    public static T Create<T>(HttpClient httpClient) where T : class
    {
        return RestService.For<T>(httpClient, GetRefitSettings());
    }

    /// <summary>
    /// Gets the Refit settings with JSON serialization configuration.
    /// </summary>
    /// <returns>Refit settings with proper JSON configuration</returns>
    private static RefitSettings GetRefitSettings()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        options.Converters.Add(new AssistantMessageContentJsonConverter());
        options.Converters.Add(new AssistantToolsTypeJsonConverter());

        return new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(options)
        };
    }

    /// <summary>
    /// Creates a Refit RestService instance for streaming operations.
    /// </summary>
    /// <typeparam name="T">The API interface type</typeparam>
    /// <param name="config">Z.AI configuration</param>
    /// <param name="baseUrl">Base URL for the API endpoint</param>
    /// <returns>A configured Refit service instance for streaming</returns>
    public static T CreateForStreaming<T>(ZaiConfig config, string baseUrl) where T : class
    {
        var httpClient = ZaiHttpClient.Create(config, new Uri(baseUrl));

        // Configure for streaming with larger timeout and different buffering
        httpClient.Timeout = TimeSpan.FromMinutes(30);

        return RestService.For<T>(httpClient, GetRefitSettings());
    }
}
