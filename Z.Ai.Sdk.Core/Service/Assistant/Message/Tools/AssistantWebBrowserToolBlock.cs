using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

/// <summary>
/// Represents a web browser tool block.
/// </summary>
public record AssistantWebBrowserToolBlock : AssistantToolsType
{
    /// <summary>
    /// The URL to browse.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}