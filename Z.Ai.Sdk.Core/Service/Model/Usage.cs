using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record Usage(
    [property: JsonPropertyName("prompt_tokens")] int PromptTokens,
    [property: JsonPropertyName("completion_tokens")] int CompletionTokens,
    [property: JsonPropertyName("total_tokens")] int TotalTokens,
    [property: JsonPropertyName("total_calls")] int TotalCalls,
    [property: JsonPropertyName("prompt_tokens_details")] PromptTokensDetails PromptTokensDetails,
    [property: JsonPropertyName("completion_tokens_details")] CompletionTokensDetails CompletionTokensDetails
);