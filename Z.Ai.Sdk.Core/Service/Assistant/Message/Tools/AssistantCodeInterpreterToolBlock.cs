using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

/// <summary>
/// Represents a code interpreter tool block.
/// </summary>
public record AssistantCodeInterpreterToolBlock : AssistantToolsType
{
    /// <summary>
    /// The code to interpret.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// The input for the code interpreter.
    /// </summary>
    [JsonPropertyName("input")]
    public string? Input { get; init; }

    /// <summary>
    /// The output from the code interpreter.
    /// </summary>
    [JsonPropertyName("output")]
    public string? Output { get; init; }
}