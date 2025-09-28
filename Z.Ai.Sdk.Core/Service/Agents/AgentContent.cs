using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Agents;

/// <summary>
/// Represents the content of an agent message.
/// </summary>
public record AgentContent
{
    /// <summary>
    /// Message type: text, image_url, video_url, object.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// Message content when type is text.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Message image URL when type is image_url.
    /// </summary>
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; init; }

    /// <summary>
    /// Message video URL when type is video_url.
    /// </summary>
    [JsonPropertyName("video_url")]
    public string? VideoUrl { get; init; }

    /// <summary>
    /// Message object when type is object.
    /// </summary>
    [JsonPropertyName("object")]
    public object? Object { get; init; }

    /// <summary>
    /// Creates a text content object.
    /// </summary>
    public static AgentContent FromText(string text) => new() { Type = "text", Text = text };

    /// <summary>
    /// Creates an image URL content object.
    /// </summary>
    public static AgentContent FromImageUrl(string imageUrl) => new() { Type = "image_url", ImageUrl = imageUrl };

    /// <summary>
    /// Creates a video URL content object.
    /// </summary>
    public static AgentContent FromVideoUrl(string videoUrl) => new() { Type = "video_url", VideoUrl = videoUrl };

    /// <summary>
    /// Creates a generic object content.
    /// </summary>
    public static AgentContent FromObject(object obj) => new() { Type = "object", Object = obj };
}
