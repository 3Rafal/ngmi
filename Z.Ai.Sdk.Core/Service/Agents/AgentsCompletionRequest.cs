using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Agents;

/// <summary>
/// Request parameters for agent completion API calls.
/// </summary>
public record AgentsCompletionRequest : IClientRequest<AgentsCompletionRequest>
{
    /// <summary>
    /// Agent ID.
    /// </summary>
    [JsonPropertyName("agent_id")]
    public required string AgentId { get; init; }

    /// <summary>
    /// Message body.
    /// </summary>
    [JsonPropertyName("messages")]
    public required List<AgentMessage> Messages { get; init; }

    /// <summary>
    /// Synchronous call: false, SSE call: true.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; init; }

    /// <summary>
    /// Sensitive word detection control.
    /// </summary>
    [JsonPropertyName("sensitive_word_check")]
    public SensitiveWordCheckRequest? SensitiveWordCheck { get; init; }

    /// <summary>
    /// Agent business fields.
    /// </summary>
    [JsonPropertyName("custom_variables")]
    public JsonObject? CustomVariables { get; init; }

    /// <summary>
    /// Request ID
    /// </summary>
    [JsonPropertyName("request_id")]
    public string? RequestId { get; set; }
}
