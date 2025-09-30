using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

/// <summary>
/// Represents a drawing tool block.
/// </summary>
public record AssistantDrawingToolBlock : AssistantToolsType
{
    /// <summary>
    /// The drawing prompt or description.
    /// </summary>
    [JsonPropertyName("prompt")]
    public string? Prompt { get; init; }
}