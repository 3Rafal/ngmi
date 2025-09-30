using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// This class represents an attachment with a file ID.
/// </summary>
public record AssistantAttachments
{
    /// <summary>
    /// The ID of the file attachment.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string? FileId { get; init; }
}