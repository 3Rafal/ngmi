using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record Delta(
    string Role,
    string Content,
    [property: JsonPropertyName("reasoning_content")] string ReasoningContent,
    Audio Audio,
    [property: JsonPropertyName("tool_calls")] List<ToolCalls> ToolCalls
);