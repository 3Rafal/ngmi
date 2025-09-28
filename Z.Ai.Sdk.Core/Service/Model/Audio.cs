using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record Audio(
    string Id,
    string Data,
    [property: JsonPropertyName("expires_at")] long ExpiresAt
);