using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// This class represents the usage statistics for a completion.
/// </summary>
public record AssistantCompletionUsage
{
    /// <summary>
    /// Number of tokens in the input (prompt).
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    /// <summary>
    /// Number of tokens in the output (completion).
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    /// <summary>
    /// Total number of tokens used.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }
}