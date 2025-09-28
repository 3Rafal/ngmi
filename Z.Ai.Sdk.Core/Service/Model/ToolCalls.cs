using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record ToolCalls(
    [property: JsonPropertyName("function")] ChatFunctionCall Function,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("mcp")] MCPToolCall Mcp
);