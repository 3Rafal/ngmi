using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

/// <summary>
/// Base class for assistant tool types.
/// </summary>
[JsonConverter(typeof(AssistantToolsTypeJsonConverter))]
public abstract record AssistantToolsType
{
    /// <summary>
    /// The type of the tool.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
