using System.Text.Json;
using Microsoft.Extensions.Logging;
using Z.Ai.Sdk.Core.Config;
using Z.Ai.Sdk.Core.Service.Videos;

namespace Z.Ai.Sdk.Core.Tests.Service.Videos;

/// <summary>
/// VideosService test class for testing various functionalities of VideosService and
/// VideosService implementation
/// Preserves 100% of Java logic from VideosServiceTest
/// </summary>
public class VideosServiceTest
{
    private readonly ILogger<VideosServiceTest> _logger;
    private IVideosService? _videosService;

    // Request ID template
    private static readonly string RequestIdTemplate = "video-test-{0}";

    public VideosServiceTest()
    {
        _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<VideosServiceTest>();
    }

    private void InitializeVideosService()
    {
        var zaiConfig = new ZaiConfig();
        var apiKey = zaiConfig.ApiKey;
        if (apiKey == string.Empty)
            zaiConfig.WithApiKey("id.test-api-key");

        var client = new ZaiClient(zaiConfig);
        _videosService = client.Videos();
    }

    [Fact]
    public void TestVideosServiceInstantiation()
    {
        // Arrange
        InitializeVideosService();

        // Assert
        Assert.NotNull(_videosService);
        Assert.IsType<VideosService>(_videosService);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestVideoGeneration()
    {
        // Arrange
        InitializeVideosService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new VideoCreateParams
        {
            Model = Constants.ModelCogVideoX3,
            Prompt = "A beautiful sunset over the ocean with waves gently crashing on the shore",
            RequestId = requestId,
            WithAudio = true,
            Quality = "speed",
            Duration = 5
        };

        // Execute test
        var response = await _videosService!.VideoGenerationsAsync(request);

        // Verify results
        Assert.NotNull(response);
        Assert.Equal(200, response.Code);
        Assert.True(response.Success, $"Video generation failed: {response.Msg}");
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        Assert.Null(response.Error);

        _logger.LogInformation("Video generation response: {Response}", JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestVideoGenerationResult()
    {
        // Arrange
        InitializeVideosService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        // First create a video generation request
        var request = new VideoCreateParams
        {
            Model = Constants.ModelCogVideoX3,
            Prompt = "A person walking in a beautiful garden",
            RequestId = requestId
        };

        var createResponse = await _videosService!.VideoGenerationsAsync(request);

        // Verify creation response
        Assert.NotNull(createResponse);
        Assert.True(createResponse.Success, $"Video creation failed: {createResponse.Msg}");
        Assert.NotNull(createResponse.Data);
        Assert.NotNull(createResponse.Data.Id);

        // Retrieve the result using task ID
        var taskId = createResponse.Data.Id;
        var resultResponse = await _videosService.VideoGenerationsResultAsync(taskId!);

        // Verify result response
        Assert.NotNull(resultResponse);
        Assert.Equal(200, resultResponse.Code);
        Assert.NotNull(resultResponse.Data);
        Assert.NotNull(resultResponse.Data.Id);

        _logger.LogInformation("Video generation result: taskId={TaskId}, response={Response}",
            taskId, JsonSerializer.Serialize(resultResponse));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestVideoGenerationResultError()
    {
        // Arrange
        InitializeVideosService();
        var mockTaskId = "mock-task-id-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Execute test
        var response = await _videosService!.VideoGenerationsResultAsync(mockTaskId);

        // Verify result
        Assert.NotNull(response);
        // For non-existent task, we expect either an error or unsuccessful response
        if (!response.Success)
        {
            Assert.NotNull(response.Error);
        }

        _logger.LogInformation("Video generation result error test: taskId={TaskId}, response={Response}",
            mockTaskId, JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestDifferentModel_CogVideoX3()
    {
        // Arrange
        InitializeVideosService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new VideoCreateParams
        {
            Model = Constants.ModelCogVideoX3,
            Prompt = "A cat playing with a ball of yarn",
            RequestId = requestId
        };

        // Execute test
        var response = await _videosService!.VideoGenerationsAsync(request);

        // Verify results
        Assert.NotNull(response);
        Assert.Equal(200, response.Code);

        _logger.LogInformation("Model {Model} response: {Response}", Constants.ModelCogVideoX3, JsonSerializer.Serialize(response));
    }

    [Fact]
    public async Task TestValidation_NullRequest()
    {
        // Arrange
        InitializeVideosService();

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _videosService!.VideoGenerationsAsync(null!));
    }

    [Fact]
    public async Task TestValidation_NullModel()
    {
        // Arrange
        InitializeVideosService();
        var request = new VideoCreateParams
        {
            Prompt = "Test video prompt"
        };

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _videosService!.VideoGenerationsAsync(request));
    }

    [Fact]
    public async Task TestValidation_EmptyPrompt()
    {
        // Arrange
        InitializeVideosService();
        var request = new VideoCreateParams
        {
            Model = Constants.ModelCogVideoX3,
            Prompt = string.Empty
        };

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _videosService!.VideoGenerationsAsync(request));
    }

    [Fact]
    public async Task TestValidation_NullTaskId()
    {
        // Arrange
        InitializeVideosService();

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _videosService!.VideoGenerationsResultAsync(null!));
    }

    [Fact]
    public async Task TestValidation_EmptyTaskId()
    {
        // Arrange
        InitializeVideosService();

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _videosService!.VideoGenerationsResultAsync(string.Empty));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestVideoGenerationWithImage()
    {
        // Arrange
        InitializeVideosService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        // For this test, we'll use a simple base64 encoded image placeholder
        // In a real scenario, you would load an actual image file
        var imageBytes = System.Text.Encoding.UTF8.GetBytes("fake-image-data-for-testing");
        var imageUrl = Convert.ToBase64String(imageBytes);

        var request = new VideoCreateParams
        {
            Model = Constants.ModelCogVideoX3,
            Prompt = "Transform this image into a dynamic video scene",
            ImageUrl = imageUrl,
            RequestId = requestId,
            WithAudio = false,
            Duration = 5
        };

        // Execute test
        var response = await _videosService!.VideoGenerationsAsync(request);

        // Verify results (this might fail with fake data, but test structure is preserved)
        Assert.NotNull(response);

        _logger.LogInformation("Video generation with image response: {Response}", JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestVideoGenerationWithCustomSettings()
    {
        // Arrange
        InitializeVideosService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new VideoCreateParams
        {
            Model = Constants.ModelCogVideoX3,
            Prompt = "A futuristic city with flying cars and neon lights",
            RequestId = requestId,
            Quality = "speed",
            WithAudio = true,
            Size = "1280x720",
            Duration = 5,
            Fps = 30
        };

        // Execute test
        var response = await _videosService!.VideoGenerationsAsync(request);

        // Verify results
        Assert.NotNull(response);
        Assert.Equal(200, response.Code);

        _logger.LogInformation("Video generation with custom settings response: {Response}", JsonSerializer.Serialize(response));
    }
}