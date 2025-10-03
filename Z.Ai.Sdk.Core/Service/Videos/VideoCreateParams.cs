using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Videos;

/// <summary>
/// Parameters for video creation API calls. This class contains all the necessary
/// parameters for generating videos, including model selection, prompts, image inputs, and
/// various video settings.
/// </summary>
public record VideoCreateParams : IClientRequest<VideoCreateParams>
{
    /// <summary>
    /// Model ID
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Model name
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; init; } = string.Empty;

    /// <summary>
    /// Text description of the required video
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; init; } = string.Empty;

    /// <summary>
    /// Supports URL or Base64, input image for image-to-video generation Image format:
    /// Image size:
    /// </summary>
    [JsonPropertyName("image_url")]
    public object? ImageUrl { get; init; }

    /// <summary>
    /// Call specified model to optimize the prompt, recommend using GLM-4-Air and
    /// GLM-4-Flash. If not specified, use the original prompt directly.
    /// </summary>
    [JsonPropertyName("prompt_opt_model")]
    public string? PromptPptModel { get; init; }

    /// <summary>
    /// Passed by the client, must ensure uniqueness; used to distinguish the unique
    /// identifier of each request, the platform will generate by default when the client
    /// does not pass.
    /// </summary>
    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }

    /// <summary>
    /// User ID
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; init; }

    /// <summary>
    /// Video quality setting
    /// </summary>
    [JsonPropertyName("quality")]
    public string? Quality { get; init; }

    /// <summary>
    /// Whether to include audio in the generated video
    /// </summary>
    [JsonPropertyName("with_audio")]
    public bool? WithAudio { get; init; }

    /// <summary>
    /// Video size/resolution
    /// </summary>
    [JsonPropertyName("size")]
    public string? Size { get; init; }

    /// <summary>
    /// style anime general
    /// </summary>
    [JsonPropertyName("style")]
    public string? Style { get; init; }

    /// <summary>
    /// Video duration in seconds
    /// </summary>
    [JsonPropertyName("duration")]
    public int? Duration { get; init; }

    /// <summary>
    /// Frames per second for the video
    /// </summary>
    [JsonPropertyName("fps")]
    public int? Fps { get; init; }

    /// <summary>
    /// Sensitive word detection control
    /// </summary>
    [JsonPropertyName("sensitive_word_check")]
    public SensitiveWordCheckRequest? SensitiveWordCheck { get; init; }

    [JsonPropertyName("movement_amplitude")]
    public string? MovementAmplitude { get; init; }

    /// <summary>
    /// 16:9 : 16:9、9:16、1:1
    /// </summary>
    [JsonPropertyName("aspect_ratio")]
    public string? AspectRatio { get; init; }

    /// <summary>
    /// Forced watermark switch
    /// </summary>
    [JsonPropertyName("watermark_enabled")]
    public bool? WatermarkEnabled { get; init; }

    /// <summary>
    /// Is it necessary to perform off peak execution
    /// </summary>
    [JsonPropertyName("off_peak")]
    public bool? OffPeak { get; init; }
}