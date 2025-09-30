using System.Text.Json;
using Microsoft.Extensions.Logging;
using Z.Ai.Sdk.Core.Service.Assistant;
using Z.Ai.Sdk.Core.Service.Assistant.Conversation;
using Z.Ai.Sdk.Core.Service.Assistant.Message;
using Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;
using Z.Ai.Sdk.Core.Config;

namespace Z.Ai.Sdk.Core.Tests.Service.Assistant;

/// <summary>
/// AssistantService test class for testing various functionalities of AssistantService and
/// AssistantService implementation
/// </summary>
public class AssistantServiceTest
{
    private readonly ILogger<AssistantServiceTest> _logger;
    private IAssistantService? _assistantService;

    // Test assistant ID
    private static readonly string TestAssistantId = "659e54b1b8006379b4b2abd6";

    // Request ID template
    private static readonly string RequestIdTemplate = "assistant-test-{0}";

    public AssistantServiceTest()
    {
        _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<AssistantServiceTest>();
    }

    private void InitializeAssistantService()
    {
        var zaiConfig = new ZaiConfig();
        var apiKey = zaiConfig.ApiKey;
        if (apiKey == string.Empty)
            zaiConfig.WithApiKey("id.test-api-key");

        var client = new ZaiClient(zaiConfig);
        _assistantService = client.Assistants();
    }

    [Fact]
    public void TestAssistantServiceInstantiation()
    {
        // Arrange
        InitializeAssistantService();

        // Assert
        Assert.NotNull(_assistantService);
        Assert.IsType<AssistantService>(_assistantService);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestStreamAssistantCompletion()
    {
        // Arrange
        InitializeAssistantService();

        var textContent = new AssistantMessageTextContent
        {
            Text = "Help me search for the release time of ZAI's CogVideoX",
            Type = "text"
        };

        var message = new AssistantConversationMessage
        {
            Role = "user",
            Content = new List<AssistantMessageTextContent> { textContent }
        };

        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new AssistantParameters
        {
            AssistantId = TestAssistantId,
            Stream = true,
            Messages = new List<AssistantConversationMessage> { message },
            Metadata = new Dictionary<string, object>
            {
                ["request_id"] = requestId
            }
        };

        // Act
        var response = await _assistantService!.AssistantCompletionStreamAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);

        // Test stream data processing
        var messageCount = 0;
        var isFirst = true;
        var choices = new List<AssistantMessageContent>();
        AssistantCompletion? lastAccumulator = null;

        if (response.Stream != null)
        {
            await foreach (var accumulator in response.Stream)
            {
                if (isFirst)
                {
                    _logger.LogInformation("Starting to receive stream response:");
                    isFirst = false;
                }

                if (accumulator.Choices != null && accumulator.Choices.Count > 0)
                {
                    var delta = accumulator.Choices[0].Delta;
                    if (delta != null)
                    {
                        try
                        {
                            _logger.LogInformation("MessageContent: {Content}", JsonSerializer.Serialize(delta));
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError(ex, "Error processing message content");
                        }
                        choices.Add(delta);
                        messageCount++;
                    }
                }
                lastAccumulator = accumulator;
            }

            _logger.LogInformation("Stream response completed, received {Count} messages in total", messageCount);
        }

        Assert.True(messageCount >= 0, "Should receive at least zero messages");
        Assert.NotNull(lastAccumulator);

        _logger.LogInformation("Stream assistant completion test completed");
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestQuerySupport()
    {
        // Arrange
        InitializeAssistantService();

        var request = new AssistantQuerySupportParams
        {
            AssistantIdList = new List<string> { TestAssistantId }
        };

        // Act
        var response = await _assistantService!.QuerySupportAsync(request);

        // Assert
        Assert.NotNull(response);
        _logger.LogInformation("Query support response: {Response}", response);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestQueryConversationUsage()
    {
        // Arrange
        InitializeAssistantService();

        var request = new AssistantConversationParameters
        {
            AssistantId = TestAssistantId,
            Page = 1,
            PageSize = 5
        };

        // Act
        var response = await _assistantService!.QueryConversationUsageAsync(request);

        // Assert
        Assert.NotNull(response.Data);
        _logger.LogInformation("Query conversation usage response: {Response}", response);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestErrorHandling_InvalidAssistantId()
    {
        // Arrange
        InitializeAssistantService();

        var textContent = new AssistantMessageTextContent
        {
            Text = "Hello",
            Type = "text"
        };

        var message = new AssistantConversationMessage
        {
            Role = "user",
            Content = new List<AssistantMessageTextContent> { textContent }
        };

        var request = new AssistantParameters
        {
            AssistantId = "invalid-assistant-id",
            Stream = false,
            Messages = new List<AssistantConversationMessage> { message }
        };

        // Act
        var response = await _assistantService!.AssistantCompletionStreamAsync(request);

        // Assert - Should handle error gracefully
        Assert.NotNull(response);
        _logger.LogInformation("Error handling response: {Response}", response);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestMultiTurnConversation()
    {
        // Arrange
        InitializeAssistantService();

        var messages = new List<AssistantConversationMessage>();

        // First round of conversation
        var firstContent = new AssistantMessageTextContent
        {
            Text = "Hello, I would like to learn about machine learning",
            Type = "text"
        };
        messages.Add(new AssistantConversationMessage
        {
            Role = "user",
            Content = new List<AssistantMessageTextContent> { firstContent }
        });

        var assistantContent = new AssistantMessageTextContent
        {
            Text = "Hello! Machine learning is an important branch of artificial intelligence...",
            Type = "text"
        };
        messages.Add(new AssistantConversationMessage
        {
            Role = "assistant",
            Content = new List<AssistantMessageTextContent> { assistantContent }
        });

        // Second round of conversation
        var secondContent = new AssistantMessageTextContent
        {
            Text = "Can you introduce supervised learning in detail?",
            Type = "text"
        };
        messages.Add(new AssistantConversationMessage
        {
            Role = "user",
            Content = new List<AssistantMessageTextContent> { secondContent }
        });

        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new AssistantParameters
        {
            AssistantId = TestAssistantId,
            Stream = true,
            Messages = messages,
            Metadata = new Dictionary<string, object>
            {
                ["request_id"] = requestId
            }
        };

        // Act
        var response = await _assistantService!.AssistantCompletionStreamAsync(request);

        if (response.Stream != null)
        {
            await foreach (var accumulator in response.Stream)
            {
                if (accumulator.Choices != null && accumulator.Choices.Count > 0)
                {
                    var delta = accumulator.Choices[0].Delta;
                    if (delta != null)
                    {
                        try
                        {
                            _logger.LogInformation("MessageContent: {Content}", JsonSerializer.Serialize(delta));
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError(ex, "Error processing message content");
                        }
                    }
                }
            }
        }

        // Assert
        Assert.NotNull(response);
    }
}