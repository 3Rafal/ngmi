using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// This class represents error information.
/// </summary>
public record AssistantErrorInfo
{
    /// <summary>
    /// Error code.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// Error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}