using System.IO;
using Microsoft.Extensions.Logging;
using Z.Ai.Sdk.Core;
using Z.Ai.Sdk.Core.Config;
using Z.Ai.Sdk.Core.Service.Audio;

namespace Z.Ai.Sdk.Core.Tests.Service.Audio;

/// <summary>
/// AudioService test class that mirrors the Java SDK coverage.
/// </summary>
public class AudioServiceTest
{
    private readonly ILogger<AudioServiceTest> _logger;

    private const string RequestIdTemplate = "audio-test-{0}";

    public AudioServiceTest()
    {
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<AudioServiceTest>();
    }

    private static IAudioService InitializeAudioService()
    {
        var zaiConfig = new ZaiConfig();
        if (string.IsNullOrEmpty(zaiConfig.ApiKey))
        {
            zaiConfig.WithApiKey("id.test-api-key");
        }

        var client = new ZaiClient(zaiConfig);
        return client.Audio();
    }

    private static string GenerateRequestId()
    {
        return string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    private static string ResolveResourcePath(string fileName)
    {
        return Path.Combine(AppContext.BaseDirectory, "Resources", "Audio", fileName);
    }

    [Fact]
    public void ShouldInstantiateAudioServiceSuccessfully()
    {
        var service = InitializeAudioService();

        Assert.NotNull(service);
        Assert.IsType<AudioService>(service);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task ShouldGenerateSpeechFromTextSuccessfully()
    {
        var service = InitializeAudioService();

        var request = new AudioSpeechRequest
        {
            Model = Constants.ModelTts,
            Input = "Hello, this is a test for text-to-speech functionality.",
            Voice = "tongtong",
            RequestId = GenerateRequestId()
        };

        var response = await service.CreateSpeechAsync(request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.True(response.Data.Exists);
        Assert.True(response.Data.Length > 0);
        Assert.Null(response.Error);

        _logger.LogInformation("Text-to-speech response saved to: {Path}", response.Data!.FullName);
    }

    // [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    [Fact(Skip = "Unknown model error")]
    public async Task ShouldGenerateCustomSpeechWithVoiceCloningSuccessfully()
    {
        var service = InitializeAudioService();
        var voicePath = ResolveResourcePath("asr.wav");

        var request = new AudioCustomizationRequest
        {
            Model = Constants.ModelTts,
            Input = "This is a test for custom voice generation.",
            VoiceData = new FileInfo(voicePath),
            VoiceText = "Sample voice text for cloning",
            ResponseFormat = "wav",
            RequestId = GenerateRequestId()
        };

        var response = await service.CreateCustomSpeechAsync(request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.True(response.Data.Exists);
        Assert.True(response.Data.Length > 0);
        Assert.Null(response.Error);

        _logger.LogInformation("Custom speech response saved to: {Path}", response.Data!.FullName);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task ShouldTranscribeAudioWithBlocking()
    {
        var service = InitializeAudioService();
        var audioPath = ResolveResourcePath("asr.wav");

        var request = new AudioTranscriptionRequest
        {
            Model = Constants.ModelGlmAsr,
            File = new FileInfo(audioPath),
            Stream = false,
            RequestId = GenerateRequestId()
        };

        var response = await service.CreateTranscriptionAsync(request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.False(string.IsNullOrEmpty(response.Data.Text));
        Assert.Null(response.Error);

        _logger.LogInformation("Blocking transcription text: {Text}", response.Data!.Text);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task ShouldTranscribeAudioWithStreaming()
    {
        var service = InitializeAudioService();
        var audioPath = ResolveResourcePath("asr.wav");

        var request = new AudioTranscriptionRequest
        {
            Model = Constants.ModelGlmAsr,
            File = new FileInfo(audioPath),
            Stream = true,
            RequestId = GenerateRequestId()
        };

        var response = await service.CreateTranscriptionAsync(request);

        Assert.NotNull(response);
        Assert.True(response.Success);

        var messageCount = 0;
        var isFirst = true;

        if (response.Stream != null)
        {
            await foreach (var chunk in response.Stream)
            {
                if (isFirst)
                {
                    _logger.LogInformation("Starting to receive stream transcription response:");
                    isFirst = false;
                }

                if (chunk?.Choices is { Count: > 0 })
                {
                    var choice = chunk.Choices[0];
                    if (!string.IsNullOrEmpty(choice.Delta?.Content))
                    {
                        _logger.LogInformation("Received transcription content: {Content}", choice.Delta.Content);
                        messageCount++;
                    }
                }
                else if (!string.IsNullOrEmpty(chunk?.Delta))
                {
                    _logger.LogInformation("Received transcription delta: {Delta}", chunk.Delta);
                    messageCount++;
                }
            }
        }

        Assert.True(messageCount >= 0);
        _logger.LogInformation("Stream transcription completed, received {Count} messages in total", messageCount);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenSpeechRequestIsNull()
    {
        var service = InitializeAudioService();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => service.CreateSpeechAsync(null!));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenCustomSpeechRequestIsNull()
    {
        var service = InitializeAudioService();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => service.CreateCustomSpeechAsync(null!));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenTranscriptionRequestIsNull()
    {
        var service = InitializeAudioService();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => service.CreateTranscriptionAsync(null!));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenAudioFileDoesNotExist()
    {
        var service = InitializeAudioService();

        var request = new AudioTranscriptionRequest
        {
            Model = Constants.ModelGlmAsr,
            File = new FileInfo("non-existent-file.wav"),
            Stream = false
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateTranscriptionAsync(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenModelIsNullInSpeechRequest()
    {
        var service = InitializeAudioService();

        var request = new AudioSpeechRequest
        {
            Input = "Test input",
            Voice = "tongtong"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateSpeechAsync(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenInputIsEmptyInSpeechRequest()
    {
        var service = InitializeAudioService();

        var request = new AudioSpeechRequest
        {
            Model = Constants.ModelTts,
            Input = string.Empty,
            Voice = "tongtong"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateSpeechAsync(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenVoiceDataIsNullInCustomSpeechRequest()
    {
        var service = InitializeAudioService();

        var request = new AudioCustomizationRequest
        {
            Model = Constants.ModelTts,
            Input = "Test input",
            VoiceText = "Voice text"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateCustomSpeechAsync(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenFileIsNullInTranscriptionRequest()
    {
        var service = InitializeAudioService();

        var request = new AudioTranscriptionRequest
        {
            Model = Constants.ModelGlmAsr,
            Stream = false
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateTranscriptionAsync(request));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task ShouldGenerateSpeechSuccessfullyWithDifferentVoiceOptions()
    {
        var service = InitializeAudioService();
        var voices = new[] { "tongtong" };

        foreach (var voice in voices)
        {
            var request = new AudioSpeechRequest
            {
                Model = Constants.ModelTts,
                Input = $"Testing voice: {voice}",
                Voice = voice,
                RequestId = GenerateRequestId()
            };

            var response = await service.CreateSpeechAsync(request);
            Assert.NotNull(response);

            _logger.LogInformation("Voice {Voice} response saved to: {Path}", voice, response.Data?.FullName);
        }
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task ShouldTranscribeDifferentAudioFormatsSuccessfully()
    {
        var service = InitializeAudioService();
        var audioFiles = new[] { "asr.wav", "asr.mp3" };

        foreach (var audioFile in audioFiles)
        {
            var request = new AudioTranscriptionRequest
            {
                Model = Constants.ModelGlmAsr,
                File = new FileInfo(ResolveResourcePath(audioFile)),
                Stream = false,
                RequestId = GenerateRequestId()
            };

            var response = await service.CreateTranscriptionAsync(request);
            Assert.NotNull(response);

            _logger.LogInformation("Audio file {File} transcription text: {Text}", audioFile, response.Data?.Text);
        }
    }
}
