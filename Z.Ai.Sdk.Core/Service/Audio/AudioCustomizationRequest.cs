using System.IO;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Request parameters for audio customization API calls.
/// </summary>
public record AudioCustomizationRequest : CommonRequestBase, IClientRequest<AudioCustomizationRequest>
{
    /// <summary>
    /// Text to generate audio from.
    /// </summary>
    [JsonPropertyName("input")]
    public string? Input { get; init; }

    /// <summary>
    /// Model code to call.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; init; }

    /// <summary>
    /// Text description of the original audio to clone.
    /// </summary>
    [JsonPropertyName("voice_text")]
    public string? VoiceText { get; init; }

    /// <summary>
    /// Original audio file to clone.
    /// </summary>
    [JsonIgnore]
    public FileInfo? VoiceData { get; init; }

    /// <summary>
    /// Audio response format.
    /// </summary>
    [JsonPropertyName("response_format")]
    public string? ResponseFormat { get; init; }

    /// <summary>
    /// Sensitive word detection control.
    /// </summary>
    [JsonPropertyName("sensitive_word_check")]
    public SensitiveWordCheckRequest? SensitiveWordCheck { get; init; }
}
