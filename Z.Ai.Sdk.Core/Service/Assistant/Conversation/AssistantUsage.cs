using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Conversation;

/// <summary>
/// This class represents the usage statistics for a conversation.
/// </summary>
public record AssistantUsage
{
    /// <summary>
    /// The number of tokens in the user's input.
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    /// <summary>
    /// The number of tokens in the model's input.
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    /// <summary>
    /// The total number of tokens.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }
}