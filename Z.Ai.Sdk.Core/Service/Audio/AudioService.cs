using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Z.Ai.Sdk.Core.Api.Audio;

namespace Z.Ai.Sdk.Core.Service.Audio;

public class AudioService : IAudioService
{
    private readonly AbstractAiClient _client;
    private readonly IAudioApi _audioApi;
    private readonly IAudioApi _streamingAudioApi;

    public AudioService(AbstractAiClient client)
    {
        _client = client;
        _audioApi = client.CreateRefitService<IAudioApi>();
        _streamingAudioApi = client.CreateRefitStreamingService<IAudioApi>();
    }

    public Task<AudioSpeechResponse> CreateSpeechAsync(AudioSpeechRequest request)
    {
        ValidateSpeechParams(request);
        return _client.ExecuteRequest<FileInfo, AudioSpeechRequest, AudioSpeechRequest, AudioSpeechResponse>(
            request,
            ExecuteSpeechRequestAsync,
            typeof(AudioSpeechResponse));
    }

    public Task<AudioCustomizationResponse> CreateCustomSpeechAsync(AudioCustomizationRequest request)
    {
        ValidateCustomSpeechParams(request);
        return _client.ExecuteRequest<FileInfo, AudioCustomizationRequest, AudioCustomizationRequest, AudioCustomizationResponse>(
            request,
            ExecuteCustomSpeechRequestAsync,
            typeof(AudioCustomizationResponse));
    }

    public Task<AudioTranscriptionResponse> CreateTranscriptionAsync(AudioTranscriptionRequest request)
    {
        ValidateTranscriptionParams(request);
        return request.Stream == true
            ? StreamTranscriptionAsync(request)
            : BlockTranscriptionAsync(request);
    }

    private Task<AudioTranscriptionResponse> StreamTranscriptionAsync(AudioTranscriptionRequest request)
    {
        return _client.BiStreamRequest<AudioTranscriptionResult, AudioTranscriptionChunk, AudioTranscriptionRequest, AudioTranscriptionRequest, AudioTranscriptionResponse>(
            request,
            ExecuteTranscriptionStreamRequestAsync,
            typeof(AudioTranscriptionResponse),
            typeof(AudioTranscriptionChunk));
    }

    private Task<AudioTranscriptionResponse> BlockTranscriptionAsync(AudioTranscriptionRequest request)
    {
        return _client.ExecuteRequest<AudioTranscriptionResult, AudioTranscriptionRequest, AudioTranscriptionRequest, AudioTranscriptionResponse>(
            request,
            ExecuteTranscriptionBlockRequestAsync,
            typeof(AudioTranscriptionResponse));
    }

    private async Task<FileInfo> ExecuteSpeechRequestAsync(AudioSpeechRequest request)
    {
        using var responseStream = await _audioApi.CreateSpeech(request).ConfigureAwait(false);
        var outputPath = CreateTemporaryFilePath("audio_speech", request.ResponseFormat);
        await WriteStreamToFileAsync(responseStream, outputPath).ConfigureAwait(false);
        return new FileInfo(outputPath);
    }

    private async Task<FileInfo> ExecuteCustomSpeechRequestAsync(AudioCustomizationRequest request)
    {
        var content = BuildCustomizationContent(request);
        try
        {
            using var responseStream = await _audioApi.CreateCustomSpeech(content).ConfigureAwait(false);
            var outputPath = CreateTemporaryFilePath("audio_customization", request.ResponseFormat);
            await WriteStreamToFileAsync(responseStream, outputPath).ConfigureAwait(false);
            return new FileInfo(outputPath);
        }
        finally
        {
            content.Dispose();
        }
    }

    private async Task<AudioTranscriptionResult> ExecuteTranscriptionBlockRequestAsync(AudioTranscriptionRequest request)
    {
        var content = BuildTranscriptionContent(request, streamOverride: request.Stream);
        try
        {
            return await _audioApi.CreateTranscription(content).ConfigureAwait(false);
        }
        finally
        {
            content.Dispose();
        }
    }

    private Task<Stream> ExecuteTranscriptionStreamRequestAsync(AudioTranscriptionRequest request)
    {
        var content = BuildTranscriptionContent(request, streamOverride: true);
        return SendStreamRequestAsync(content, _streamingAudioApi.CreateTranscriptionStream);
    }

    private static async Task<Stream> SendStreamRequestAsync(
        MultipartFormDataContent content,
        Func<MultipartFormDataContent, Task<Stream>> sender)
    {
        try
        {
            return await sender(content).ConfigureAwait(false);
        }
        finally
        {
            content.Dispose();
        }
    }

    private static MultipartFormDataContent BuildCustomizationContent(AudioCustomizationRequest request)
    {
        var content = new MultipartFormDataContent();

        AddStringContent(content, "input", request.Input);
        AddStringContent(content, "model", request.Model);
        AddStringContent(content, "voice_text", request.VoiceText);
        AddStringContent(content, "response_format", request.ResponseFormat);
        AddStringContent(content, "request_id", request.RequestId);
        AddStringContent(content, "user_id", request.UserId);

        if (request.SensitiveWordCheck != null)
        {
            var json = JsonSerializer.Serialize(request.SensitiveWordCheck);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            content.Add(jsonContent, "sensitive_word_check");
        }

        var voiceData = request.VoiceData!;
        var voiceStream = System.IO.File.OpenRead(voiceData.FullName);
        var streamContent = new StreamContent(voiceStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(DetectContentType(voiceData));
        content.Add(streamContent, "voice_data", voiceData.Name);

        return content;
    }

    private static MultipartFormDataContent BuildTranscriptionContent(
        AudioTranscriptionRequest request,
        bool? streamOverride = null)
    {
        var content = new MultipartFormDataContent();

        AddStringContent(content, "model", request.Model);

        var streamValue = streamOverride ?? request.Stream;
        if (streamValue.HasValue)
        {
            AddStringContent(content, "stream", streamValue.Value ? "true" : "false");
        }

        AddStringContent(content, "request_id", request.RequestId);
        AddStringContent(content, "user_id", request.UserId);

        if (request.Temperature.HasValue)
        {
            AddStringContent(
                content,
                "temperature",
                request.Temperature.Value.ToString(CultureInfo.InvariantCulture));
        }

        var file = request.File!;
        var fileStream = System.IO.File.OpenRead(file.FullName);
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(DetectContentType(file));
        content.Add(streamContent, "file", file.Name);

        return content;
    }

    private static async Task WriteStreamToFileAsync(Stream source, string filePath)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await source.CopyToAsync(fileStream).ConfigureAwait(false);
    }

    private static void AddStringContent(MultipartFormDataContent content, string name, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        content.Add(new StringContent(value, Encoding.UTF8), name);
    }

    private static string CreateTemporaryFilePath(string prefix, string? responseFormat)
    {
        var extension = GetAudioExtension(responseFormat);
        var fileName = $"{prefix}_{Guid.NewGuid():N}{extension}";
        return Path.Combine(Path.GetTempPath(), fileName);
    }

    private static string GetAudioExtension(string? responseFormat)
    {
        return responseFormat?.Trim().ToLowerInvariant() switch
        {
            "mp3" => ".mp3",
            "wav" => ".wav",
            "m4a" => ".m4a",
            "aac" => ".aac",
            "ogg" => ".ogg",
            "flac" => ".flac",
            "wma" => ".wma",
            _ => ".wav"
        };
    }

    private static string DetectContentType(FileInfo file)
    {
        return file.Extension.ToLowerInvariant() switch
        {
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".m4a" => "audio/mp4",
            ".aac" => "audio/aac",
            ".ogg" => "audio/ogg",
            ".flac" => "audio/flac",
            ".wma" => "audio/x-ms-wma",
            _ => "audio/mpeg"
        };
    }

    private static void ValidateSpeechParams(AudioSpeechRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "request cannot be null");
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            throw new ArgumentException("request model cannot be null", nameof(request.Model));
        }

        if (string.IsNullOrWhiteSpace(request.Input))
        {
            throw new ArgumentException("request input cannot be null or empty", nameof(request.Input));
        }
    }

    private static void ValidateCustomSpeechParams(AudioCustomizationRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "request cannot be null");
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            throw new ArgumentException("request model cannot be null", nameof(request.Model));
        }

        if (string.IsNullOrWhiteSpace(request.Input))
        {
            throw new ArgumentException("request input cannot be null or empty", nameof(request.Input));
        }

        if (request.VoiceData == null)
        {
            throw new ArgumentException("request voice data cannot be null", nameof(request.VoiceData));
        }

        if (!request.VoiceData.Exists)
        {
            throw new ArgumentException("request voice data does not exist", nameof(request.VoiceData));
        }
    }

    private static void ValidateTranscriptionParams(AudioTranscriptionRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "request cannot be null");
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            throw new ArgumentException("request model cannot be null", nameof(request.Model));
        }

        if (request.File == null)
        {
            throw new ArgumentException("request file cannot be null", nameof(request.File));
        }

        if (!request.File.Exists)
        {
            throw new ArgumentException("request file does not exist", nameof(request.File));
        }
    }
}
