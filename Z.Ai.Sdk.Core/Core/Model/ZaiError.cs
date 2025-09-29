using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Model;

/// <summary>
/// Represents the error body when a ZAI request fails.
/// </summary>
public record ZaiError
{
    [JsonPropertyName("error")]
    public ZaiErrorDetails? Error { get; init; }

    [JsonPropertyName("contentFilter")]
    public List<ContentFilter>? ContentFilter { get; init; }
}

/// <summary>
/// Contains the details of a ZAI error.
/// </summary>
public record ZaiErrorDetails
{
    /// <summary>
    /// Human-readable error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    /// <summary>
    /// ZAI error code, for example "401".
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }
}

/// <summary>
/// Represents sensitive words content filter information.
/// </summary>
public record ContentFilter
{
    [JsonPropertyName("level")]
    public string? Level { get; init; }

    [JsonPropertyName("role")]
    public string? Role { get; init; }
}
