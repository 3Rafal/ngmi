using System.Collections.Generic;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Represents a streaming transcription chunk emitted during ASR processing.
/// </summary>
public record AudioTranscriptionChunk
{
    [JsonPropertyName("choices")]
    public List<Choice>? Choices { get; init; }

    [JsonPropertyName("created")]
    public long? Created { get; init; }

    [JsonPropertyName("model")]
    public string? Model { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("delta")]
    public string? Delta { get; init; }
}
