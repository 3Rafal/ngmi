
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Agents;

/// <summary>
/// Parameters for retrieving agent asynchronous task results.
/// </summary>
public record AgentAsyncResultRetrieveParams : IClientRequest<object>
{
    /// <summary>
    /// The ID of the agent async task to retrieve.
    /// </summary>
    [JsonPropertyName("task_id")]
    public required string TaskId { get; init; }

    /// <summary>
    /// The agent ID associated with the async task.
    /// </summary>
    [JsonPropertyName("agent_id")]
    public required string AgentId { get; init; }

    /// <summary>
    /// Optional request ID for tracking.
    /// </summary>
    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }
}
