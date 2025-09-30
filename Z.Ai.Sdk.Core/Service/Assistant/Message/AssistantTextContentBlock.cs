using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message;

/// <summary>
/// This class represents a block of text content in a conversation.
/// </summary>
public record AssistantTextContentBlock : AssistantMessageContent
{
    /// <summary>
    /// The content of the text block.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    /// <summary>
    /// The role of the speaker, default is "assistant".
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; init; } = "assistant";
}