using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// This class represents the text content of a message. Currently supports only type = text.
/// </summary>
public record AssistantMessageTextContent
{
    /// <summary>
    /// The type of the message content, currently only "text" is supported.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// The text content of the message.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }
}