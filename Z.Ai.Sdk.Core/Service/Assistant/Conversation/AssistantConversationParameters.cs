using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Assistant.Conversation;

/// <summary>
/// This class represents the parameters for a conversation, including pagination.
/// </summary>
public record AssistantConversationParameters : IClientRequest<AssistantConversationParameters>
{
    /// <summary>
    /// The Assistant ID.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string? AssistantId { get; init; }

    /// <summary>
    /// The current page number for pagination.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; init; }

    /// <summary>
    /// The number of items per page for pagination.
    /// </summary>
    [JsonPropertyName("page_size")]
    public int PageSize { get; init; }
}