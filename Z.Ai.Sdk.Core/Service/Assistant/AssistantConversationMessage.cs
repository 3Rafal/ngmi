using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// This class represents a conversation message body.
/// </summary>
public record AssistantConversationMessage
{
    /// <summary>
    /// The role of the user input, e.g., 'user'.
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; init; }

    /// <summary>
    /// The content of the conversation message.
    /// </summary>
    [JsonPropertyName("content")]
    public List<AssistantMessageTextContent>? Content { get; init; }
}