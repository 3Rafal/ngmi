using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record PromptTokensDetails(
    [property: JsonPropertyName("cached_tokens")] int CachedTokens
);