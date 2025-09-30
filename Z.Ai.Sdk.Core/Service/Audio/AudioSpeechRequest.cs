using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Request parameters for creating speech audio from text.
/// </summary>
public record AudioSpeechRequest : CommonRequestBase, IClientRequest<AudioSpeechRequest>
{
    /// <summary>
    /// Model code to call.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; init; }

    /// <summary>
    /// Text to generate speech from.
    /// </summary>
    [JsonPropertyName("input")]
    public string? Input { get; init; }

    /// <summary>
    /// Voice tone for speech generation.
    /// </summary>
    [JsonPropertyName("voice")]
    public string? Voice { get; init; }

    /// <summary>
    /// Format of the generated speech file.
    /// </summary>
    [JsonPropertyName("response_format")]
    public string? ResponseFormat { get; init; }

    /// <summary>
    /// Sensitive word detection control.
    /// </summary>
    [JsonPropertyName("sensitive_word_check")]
    public SensitiveWordCheckRequest? SensitiveWordCheck { get; init; }

    /// <summary>
    /// Forced watermark switch.
    /// </summary>
    [JsonPropertyName("watermark_enabled")]
    public bool? WatermarkEnabled { get; init; }

    /// <summary>
    /// Voice speed for speech generation.
    /// </summary>
    [JsonPropertyName("speed")]
    public float? Speed { get; init; }

    /// <summary>
    /// Volume of the generated speech file.
    /// </summary>
    [JsonPropertyName("volume")]
    public float? Volume { get; init; }
}
