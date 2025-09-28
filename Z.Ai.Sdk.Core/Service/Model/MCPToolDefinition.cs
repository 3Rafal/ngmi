using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record MCPToolDefinition(
    string Name,
    string Description,
    object Annotations,
    [property: JsonPropertyName("input_schema")] InputSchema InputSchema
);