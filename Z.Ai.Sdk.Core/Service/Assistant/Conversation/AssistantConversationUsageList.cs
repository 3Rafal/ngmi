using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Conversation;

/// <summary>
/// This class represents a list of conversation usage data.
/// </summary>
public record AssistantConversationUsageList
{
    /// <summary>
    /// The Assistant ID.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string? AssistantId { get; init; }

    /// <summary>
    /// Whether there are more pages of results.
    /// </summary>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; init; }

    /// <summary>
    /// The list of conversation usage data.
    /// </summary>
    [JsonPropertyName("conversation_list")]
    public List<AssistantConversationUsage>? ConversationList { get; init; }
}