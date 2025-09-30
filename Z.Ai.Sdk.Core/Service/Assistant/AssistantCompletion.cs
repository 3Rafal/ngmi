using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// This class represents the completion data returned by an assistant.
/// </summary>
public record AssistantCompletion
{
    /// <summary>
    /// Request ID
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Conversation ID
    /// </summary>
    [JsonPropertyName("conversation_id")]
    public string? ConversationId { get; init; }

    /// <summary>
    /// Assistant ID
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string? AssistantId { get; init; }

    /// <summary>
    /// Request creation time, Unix timestamp
    /// </summary>
    [JsonPropertyName("created")]
    public long? Created { get; init; }

    /// <summary>
    /// Return status, including: `completed` indicates generation finished, `in_progress`
    /// indicates generating, `failed` indicates generation exception
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Error information
    /// </summary>
    [JsonPropertyName("last_error")]
    public AssistantErrorInfo? LastError { get; init; }

    /// <summary>
    /// Incremental return information
    /// </summary>
    [JsonPropertyName("choices")]
    public List<AssistantChoice>? Choices { get; init; }

    /// <summary>
    /// Metadata, extension field
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; init; }

    /// <summary>
    /// Token count statistics
    /// </summary>
    [JsonPropertyName("usage")]
    public AssistantCompletionUsage? Usage { get; init; }
}