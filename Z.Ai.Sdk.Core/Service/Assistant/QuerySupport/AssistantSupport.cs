using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;

/// <summary>
/// This class represents the details of an assistant.
/// </summary>
public record AssistantSupport
{
    /// <summary>
    /// The Assistant ID, used for assistant conversations.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string? AssistantId { get; init; }

    /// <summary>
    /// The creation time of the assistant.
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; init; }

    /// <summary>
    /// The last update time of the assistant.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; init; }

    /// <summary>
    /// The name of the assistant.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The avatar of the assistant.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; init; }

    /// <summary>
    /// The description of the assistant.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// The status of the assistant, currently only "publish".
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// The list of tools supported by the assistant.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<string>? Tools { get; init; }

    /// <summary>
    /// The list of recommended prompts to start the assistant.
    /// </summary>
    [JsonPropertyName("starter_prompts")]
    public List<string>? StarterPrompts { get; init; }
}