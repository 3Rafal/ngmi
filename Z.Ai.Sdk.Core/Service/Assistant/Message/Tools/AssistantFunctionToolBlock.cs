using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

/// <summary>
/// Represents a function tool block.
/// </summary>
public record AssistantFunctionToolBlock : AssistantToolsType
{
    /// <summary>
    /// The function to call.
    /// </summary>
    [JsonPropertyName("function")]
    public AssistantFunction? Function { get; init; }

    /// <summary>
    /// Represents a function definition.
    /// </summary>
    public record AssistantFunction
    {
        /// <summary>
        /// The name of the function.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        /// <summary>
        /// The arguments for the function.
        /// </summary>
        [JsonPropertyName("arguments")]
        public string? Arguments { get; init; }
    }
}