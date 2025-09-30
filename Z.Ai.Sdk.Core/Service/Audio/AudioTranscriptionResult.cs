using System.Collections.Generic;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Represents the complete transcription result for non-streaming ASR requests.
/// </summary>
public record AudioTranscriptionResult
{
    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }

    [JsonPropertyName("created")]
    public long? Created { get; init; }

    [JsonPropertyName("model")]
    public string? Model { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("segments")]
    public List<Segment>? Segments { get; init; }
}
