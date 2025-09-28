using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record CompletionTokensDetails(
    [property: JsonPropertyName("reasoning_tokens")] int ReasoningTokens
);