using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using Z.Ai.Sdk.Core.Service.Audio;

namespace Z.Ai.Sdk.Core.Api.Audio;

/// <summary>
/// Audio API for speech synthesis, customization, and transcription capabilities.
/// </summary>
public interface IAudioApi
{
    /// <summary>
    /// Text-to-speech conversion.
    /// </summary>
    [Post("/audio/speech")]
    Task<Stream> CreateSpeech([Body] AudioSpeechRequest request);

    /// <summary>
    /// Audio customization with voice cloning data.
    /// </summary>
    [Post("/audio/customization")]
    Task<Stream> CreateCustomSpeech([Body] HttpContent request);

    /// <summary>
    /// Non-streaming transcription.
    /// </summary>
    [Post("/audio/transcriptions")]
    Task<AudioTranscriptionResult> CreateTranscription([Body] HttpContent request);

    /// <summary>
    /// Streaming transcription.
    /// </summary>
    [Post("/audio/transcriptions")]
    Task<Stream> CreateTranscriptionStream([Body] HttpContent request);
}
