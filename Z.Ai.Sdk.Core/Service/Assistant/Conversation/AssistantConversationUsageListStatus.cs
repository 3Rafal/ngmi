using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Conversation;

/// <summary>
/// This class represents the response containing a list of conversation usage data.
/// </summary>
public record AssistantConversationUsageListStatus
{
    /// <summary>
    /// The response code.
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; init; }

    /// <summary>
    /// The response message.
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Msg { get; init; }

    /// <summary>
    /// The data containing the conversation usage list.
    /// </summary>
    [JsonPropertyName("data")]
    public AssistantConversationUsageList? Data { get; init; }
}