using System.IO;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Request parameters for audio transcription API calls.
/// </summary>
public record AudioTranscriptionRequest : CommonRequestBase, IClientRequest<AudioTranscriptionRequest>
{
    /// <summary>
    /// Model code to call (required).
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; init; }

    /// <summary>
    /// Whether to use streaming transcription.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; init; }

    /// <summary>
    /// Audio file to be transcribed (required).
    /// </summary>
    [JsonIgnore]
    public FileInfo? File { get; init; }

    /// <summary>
    /// Sampling temperature controlling output randomness.
    /// </summary>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; init; }
}
