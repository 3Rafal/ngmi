using System.Collections.Generic;

namespace Z.Ai.Sdk.Core.Service.Model;

public record MCPToolCall(
    string Id,
    string Type,
    string ServerLabel,
    string Error,
    List<MCPToolDefinition> Tools,
    string Arguments,
    string Name,
    object Output
);