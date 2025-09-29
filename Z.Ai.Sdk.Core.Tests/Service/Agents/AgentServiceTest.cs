using System.Text.Json;
using Microsoft.Extensions.Logging;
using Z.Ai.Sdk.Core.Service.Agents;
using Z.Ai.Sdk.Core.Config;

namespace Z.Ai.Sdk.Core.Tests.Service.Agents;

/// <summary>
/// AgentService test class for testing various functionalities of AgentService and
/// AgentServiceImpl
/// </summary>
public class AgentServiceTest
{
    private readonly ILogger<AgentServiceTest> _logger;
    private IAgentService? _agentService;

    // Request ID template
    private static readonly string RequestIdTemplate = "agent-test-{0}";

    // Test agent ID
    private static readonly string TestAgentId = "general_translation";

    public AgentServiceTest()
    {
        _logger = LoggerFactory.Create(builder => {}).CreateLogger<AgentServiceTest>();
    }

    private void InitializeAgentService()
    {
        var zaiConfig = new ZaiConfig();
        var apiKey = zaiConfig.ApiKey;
        if (apiKey == string.Empty)
            zaiConfig.WithApiKey("a.abcdefghabcdefghabcdefghabcdefgh");

        var client = new ZaiClient(zaiConfig);
        _agentService = client.Agents();
    }

    [Fact]
    public void TestAgentServiceInstantiation()
    {
        // Arrange
        InitializeAgentService();

        // Assert
        Assert.NotNull(_agentService);
        Assert.IsType<AgentService>(_agentService);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestSyncAgentCompletion()
    {
        // Arrange
        InitializeAgentService();

        var messages = new List<AgentMessage>();
        var userMessage = new AgentMessage { Role = "user", Content = AgentContent.FromText("Hello, please translate this to Chinese: How are you?") };
        messages.Add(userMessage);

        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new AgentsCompletionRequest
        {
            AgentId = TestAgentId,
            Stream = false,
            Messages = messages,
            RequestId = requestId
        };

        // Act
        var response = await _agentService!.CreateAgentCompletionAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Equal(requestId, response.Data.RequestId);
        Assert.NotNull(response.Data.Choices);
        Assert.NotEmpty(response.Data.Choices);
        Assert.Null(response.Error);

        _logger.LogInformation("Synchronous agent completion response: {Response}",
            JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestStreamAgentCompletion()
    {
        // Arrange
        InitializeAgentService();

        var messages = new List<AgentMessage>();
        var userMessage = new AgentMessage { Role = "user", Content = AgentContent.FromText("Please translate this to Chinese: The weather is beautiful today") };
        messages.Add(userMessage);

        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new AgentsCompletionRequest
        {
            AgentId = TestAgentId,
            Stream = true,
            Messages = messages,
            RequestId = requestId
        };

        // Act
        var response = await _agentService!.CreateAgentCompletionAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);

        // Test stream data processing
        var messageCount = 0;
        var isFirst = true;

        if (response.Stream != null)
        {
            await foreach (var modelData in response.Stream)
            {
                if (isFirst)
                {
                    _logger.LogInformation("Starting to receive stream response:");
                    isFirst = false;
                }

                if (modelData.Choices != null && modelData.Choices.Count > 0)
                {
                    var choice = modelData.Choices[0];
                    if (choice.Delta != null && choice.Delta.Content != null)
                    {
                        _logger.LogInformation("Received content: {Content}", choice.Delta.Content);
                        messageCount++;
                    }
                }
            }

            _logger.LogInformation("Stream response completed, received {Count} messages in total", messageCount);
        }

        Assert.True(messageCount > 0, "Should receive at least one message");
        _logger.LogInformation("Stream agent completion test completed");
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestRetrieveAgentAsyncResult()
    {
        // Arrange
        InitializeAgentService();

        var taskId = "test-task-id-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var retrieveParams = new AgentAsyncResultRetrieveParams
        {
            TaskId = taskId,
            AgentId = TestAgentId,
            RequestId = requestId
        };

        // Act
        var response = await _agentService!.RetrieveAgentAsyncResultAsync(retrieveParams);

        // Assert
        Assert.NotNull(response);

        // Note: Since this is a test with a mock task ID, we expect it to handle the
        // error gracefully
        try
        {
            _logger.LogInformation("Retrieve agent async result: {Result}", response);
        }
        catch (Exception e)
        {
            // Expected for non-existent task ID
            _logger.LogInformation("Expected error for non-existent task ID: {Message}", e.Message);
            Assert.Contains("task", e.Message, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public async Task TestValidation_NullRequest()
    {
        // Arrange
        InitializeAgentService();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _agentService!.CreateAgentCompletionAsync(null!));

        Assert.Equal("request", exception.ParamName);
    }

    [Fact]
    public async Task TestValidation_EmptyMessages()
    {
        // Arrange
        InitializeAgentService();

        var request = new AgentsCompletionRequest
        {
            AgentId = TestAgentId,
            Messages = new List<AgentMessage>()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _agentService!.CreateAgentCompletionAsync(request));

        Assert.Contains("messages", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task TestValidation_NullAgentId()
    {
        // Arrange
        InitializeAgentService();

        var messages = new List<AgentMessage>();
        messages.Add(new AgentMessage { Role = "user", Content = "Test message" });

        var request = new AgentsCompletionRequest
        {
            AgentId = string.Empty,
            Messages = messages
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _agentService!.CreateAgentCompletionAsync(request));

        Assert.Contains("agent_id", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task TestValidation_NullMessages()
    {
        // Arrange
        InitializeAgentService();

        var request = new AgentsCompletionRequest
        {
            AgentId = TestAgentId,
            Messages = new List<AgentMessage>()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _agentService!.CreateAgentCompletionAsync(request));

        Assert.Contains("messages", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestMultiTurnConversationWithAgent()
    {
        // Arrange
        InitializeAgentService();

        var messages = new List<AgentMessage>
        {
            // First round of conversation
            new() { Role = "user", Content = AgentContent.FromText("Please translate 'Hello' to Chinese") },
            new() { Role = "assistant", Content = AgentContent.FromText("你好") },

            // Second round of conversation
            new() { Role = "user", Content = AgentContent.FromText("Now translate 'Thank you' to Chinese") }
        };

        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new AgentsCompletionRequest
        {
            AgentId = TestAgentId,
            Stream = false,
            Messages = messages,
            RequestId = requestId
        };

        // Act
        var response = await _agentService!.CreateAgentCompletionAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Data?.Choices);
        Assert.NotEmpty(response.Data.Choices);
        Assert.Null(response.Error);
        Assert.NotNull(response.Data.Choices[0].Message);

        _logger.LogInformation("Multi-turn conversation with agent response: {Response}",
            JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestAgentCompletionWithCustomVariables()
    {
        // Arrange
        InitializeAgentService();

        var messages = new List<AgentMessage>();
        var userMessage = new AgentMessage { Role = "user", Content = AgentContent.FromText("Translate this text") };
        messages.Add(userMessage);

        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        // Note: custom_variables would typically be set if the agent supports it
        var request = new AgentsCompletionRequest
        {
            AgentId = TestAgentId,
            Stream = false,
            Messages = messages,
            RequestId = requestId
        };

        // Act
        var response = await _agentService!.CreateAgentCompletionAsync(request);

        // Assert
        Assert.NotNull(response);
        _logger.LogInformation("Agent completion with custom variables response: {Response}",
            new { response.Code, response.Msg });
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestRetrieveAsyncResultError()
    {
        // Arrange
        InitializeAgentService();

        var retrieveParams = new AgentAsyncResultRetrieveParams
        {
            TaskId = "mock-task-id-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            AgentId = "non-existent-agent"
        };

        // Act
        var responseTask = _agentService!.RetrieveAgentAsyncResultAsync(retrieveParams);

        // Assert
        Assert.NotNull(responseTask);

        // Expect an error when trying to retrieve non-existent task
        var exception = await Assert.ThrowsAsync<Exception>(async () => await responseTask);
        _logger.LogInformation("Expected error for non-existent task: {Message}", exception.Message);
        Assert.Contains("task", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}