using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;

/// <summary>
/// This class represents the response containing a list of assistant supports.
/// </summary>
public record AssistantSupportStatus
{
    /// <summary>
    /// The response code.
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; init; }

    /// <summary>
    /// The response message.
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Msg { get; init; }

    /// <summary>
    /// The list of assistant supports.
    /// </summary>
    [JsonPropertyName("data")]
    public List<AssistantSupport>? Data { get; init; }
}