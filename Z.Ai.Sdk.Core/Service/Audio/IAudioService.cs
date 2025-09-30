namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Audio service interface exposing speech synthesis, customization, and transcription operations.
/// </summary>
public interface IAudioService
{
    /// <summary>
    /// Creates speech from text using text-to-speech.
    /// </summary>
    Task<AudioSpeechResponse> CreateSpeechAsync(AudioSpeechRequest request);

    /// <summary>
    /// Creates customized speech with specific voice characteristics.
    /// </summary>
    Task<AudioCustomizationResponse> CreateCustomSpeechAsync(AudioCustomizationRequest request);

    /// <summary>
    /// Creates audio transcription from audio files.
    /// </summary>
    Task<AudioTranscriptionResponse> CreateTranscriptionAsync(AudioTranscriptionRequest request);
}
