using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Conversation;

/// <summary>
/// This class represents the usage data for a specific conversation.
/// </summary>
public record AssistantConversationUsage
{
    /// <summary>
    /// The conversation ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The Assistant ID.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string? AssistantId { get; init; }

    /// <summary>
    /// The creation time of the conversation.
    /// </summary>
    [JsonPropertyName("create_time")]
    public string? CreateTime { get; init; }

    /// <summary>
    /// The last update time of the conversation.
    /// </summary>
    [JsonPropertyName("update_time")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// The usage statistics for the conversation.
    /// </summary>
    [JsonPropertyName("usage")]
    public AssistantUsage? Usage { get; init; }
}