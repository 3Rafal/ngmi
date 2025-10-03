using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Videos;

/// <summary>
/// Response for video operations
/// </summary>
public class VideosResponse : IClientResponse<VideoObject>
{
    /// <summary>
    /// Response code
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// Response message
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    /// <summary>
    /// Success flag
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Video data
    /// </summary>
    [JsonPropertyName("data")]
    public VideoObject? Data { get; set; }

    /// <summary>
    /// Error information
    /// </summary>
    [JsonPropertyName("error")]
    public ChatError? Error { get; set; }
}