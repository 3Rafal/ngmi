using System.Collections.Generic;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message;

/// <summary>
/// This class represents a block of tool call data in a conversation.
/// </summary>
public record AssistantToolsDeltaBlock : AssistantMessageContent
{
    /// <summary>
    /// A list of tool call types.
    /// </summary>
    [JsonPropertyName("tool_calls")]
    public List<AssistantToolsType>? ToolCalls { get; init; }

    /// <summary>
    /// The role of the speaker, default is "tool".
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; init; } = "tool";
}