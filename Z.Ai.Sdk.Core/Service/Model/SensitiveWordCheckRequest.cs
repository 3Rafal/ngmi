using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Request parameters for sensitive word checking.
/// </summary>
public record SensitiveWordCheckRequest
{
    /// <summary>
    /// Type of sensitive words to check. Currently only supports "ALL".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Status of sensitive word checking. ENABLE or DISABLE.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
