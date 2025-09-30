using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Service.Assistant.Message;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// Represents an assistant's choice output in conversation responses. This class contains
/// the response index, message content, finish reason, and metadata.
/// </summary>
public record AssistantChoice
{
    /// <summary>
    /// Result index.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; init; }

    /// <summary>
    /// Current conversation output message content.
    /// </summary>
    [JsonPropertyName("delta")]
    public AssistantMessageContent? Delta { get; init; }

    /// <summary>
    /// Reason for inference completion:
    /// - stop: inference naturally ended or triggered stop words
    /// - sensitive: model inference content was blocked by security review
    /// - network_error: model inference service exception
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; init; }

    /// <summary>
    /// Metadata, extension field.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; init; }
}