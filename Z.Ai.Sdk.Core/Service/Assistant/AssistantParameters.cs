using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// This class represents the parameters for an assistant, including optional fields.
/// </summary>
public record AssistantParameters : IClientRequest<AssistantParameters>
{
    /// <summary>
    /// The ID of the assistant.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string? AssistantId { get; init; }

    /// <summary>
    /// The conversation ID. If not provided, a new conversation is created.
    /// </summary>
    [JsonPropertyName("conversation_id")]
    public string? ConversationId { get; init; }

    /// <summary>
    /// The name of the model, default is 'GLM-4-Assistant'.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; init; }

    /// <summary>
    /// Whether to support streaming SSE, should be set to True.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; init; }

    /// <summary>
    /// The list of conversation messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<AssistantConversationMessage>? Messages { get; init; }

    /// <summary>
    /// The list of file attachments for the conversation, optional.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<AssistantAttachments>? Attachments { get; init; }

    /// <summary>
    /// Metadata or additional fields, optional.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; init; }

    [JsonPropertyName("extra_parameters")]
    public AssistantExtraParameters? ExtraParameters { get; init; }
}