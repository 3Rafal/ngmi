using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Videos;

/// <summary>
/// This class represents the result of a video creation process.
/// </summary>
public record VideoResult
{
    /// <summary>
    /// Video URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// Cover image URL
    /// </summary>
    [JsonPropertyName("cover_image_url")]
    public string CoverImageUrl { get; init; } = string.Empty;
}