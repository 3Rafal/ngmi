using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record ChatMessage(
    string Role,
    object Content,
    [property: JsonPropertyName("reasoning_content")] string ReasoningContent,
    Audio Audio,
    string Name,
    [property: JsonPropertyName("tool_calls")] List<ToolCalls> ToolCalls,
    [property: JsonPropertyName("tool_call_id")] string ToolCallId
);