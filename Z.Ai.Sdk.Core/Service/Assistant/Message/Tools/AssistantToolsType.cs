using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

/// <summary>
/// Base class for assistant tool types.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(AssistantWebBrowserToolBlock), "web_browser")]
[JsonDerivedType(typeof(AssistantRetrievalToolBlock), "retrieval")]
[JsonDerivedType(typeof(AssistantFunctionToolBlock), "function")]
[JsonDerivedType(typeof(AssistantCodeInterpreterToolBlock), "code_interpreter")]
[JsonDerivedType(typeof(AssistantDrawingToolBlock), "drawing_tool")]
public abstract record AssistantToolsType
{
    /// <summary>
    /// The type of the tool.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}