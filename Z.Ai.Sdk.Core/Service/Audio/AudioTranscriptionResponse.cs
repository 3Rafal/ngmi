using System.Collections.Generic;
using System.Linq;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Response wrapper for audio transcription API calls supporting both synchronous and streaming results.
/// </summary>
public class AudioTranscriptionResponse : IStreamableClientResponse<AudioTranscriptionResult, AudioTranscriptionChunk>
{
    public int Code { get; set; }

    public string? Msg { get; set; }

    public bool Success { get; set; }

    public AudioTranscriptionResult? Data { get; set; }

    public ChatError? Error { get; set; }

    public IAsyncEnumerable<AudioTranscriptionChunk> Stream { get; set; } = AsyncEnumerable.Empty<AudioTranscriptionChunk>();
}
