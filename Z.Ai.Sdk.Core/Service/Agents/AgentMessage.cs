using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Agents;

/// <summary>
/// Represents a message in an agent conversation.
/// </summary>
public record AgentMessage
{
    /// <summary>
    /// The role of the message sender (e.g., "user", "assistant").
    /// </summary>
    [JsonPropertyName("role")]
    public required string Role { get; init; }

    /// <summary>
    /// The content of the message, which can be a single AgentContent object or a list of them.
    /// </summary>
    [JsonPropertyName("content")]
    public required object Content { get; init; }
}
