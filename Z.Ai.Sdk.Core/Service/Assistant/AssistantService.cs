using Z.Ai.Sdk.Core.Api.Assistant;
using Z.Ai.Sdk.Core.Service.Assistant.Conversation;
using Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// Implementation of AssistantService
/// </summary>
public class AssistantService : IAssistantService
{
    private readonly AbstractAiClient _client;
    private readonly IAssistantApi _assistantApi;
    private readonly IAssistantApi _streamingAssistantApi;

    public AssistantService(AbstractAiClient client)
    {
        _client = client;
        _assistantApi = client.CreateRefitService<IAssistantApi>();
        _streamingAssistantApi = client.CreateRefitStreamingService<IAssistantApi>();
    }

    /// <summary>
    /// Creates a streaming assistant completion.
    /// </summary>
    /// <param name="request">the assistant completion request</param>
    /// <returns>AssistantApiResponse containing the completion result</returns>
    public Task<AssistantApiResponse> AssistantCompletionStreamAsync(AssistantParameters request)
    {
        return _client.StreamRequest<AssistantCompletion, AssistantParameters, AssistantParameters, AssistantApiResponse>(
            request,
            _streamingAssistantApi.AssistantCompletionStream,
            typeof(AssistantApiResponse)
        );
    }

    /// <summary>
    /// Creates a non-streaming assistant completion.
    /// </summary>
    /// <param name="request">the assistant completion request</param>
    /// <returns>AssistantApiResponse containing the completion result</returns>
    public Task<AssistantApiResponse> AssistantCompletionAsync(AssistantParameters request)
    {
        return _client.ExecuteRequest<AssistantCompletion, AssistantParameters, AssistantParameters, AssistantApiResponse>(
            request,
            _assistantApi.AssistantCompletion,
            typeof(AssistantApiResponse)
        );
    }

    /// <summary>
    /// Queries assistant support status.
    /// </summary>
    /// <param name="request">the query support request</param>
    /// <returns>AssistantSupportResponse containing the support information</returns>
    public Task<AssistantSupportResponse> QuerySupportAsync(AssistantQuerySupportParams request)
    {
        return _client.ExecuteRequest<AssistantSupportStatus, AssistantQuerySupportParams, AssistantQuerySupportParams, AssistantSupportResponse>(
            request,
            _assistantApi.QuerySupport,
            typeof(AssistantSupportResponse)
        );
    }

    /// <summary>
    /// Queries conversation usage information.
    /// </summary>
    /// <param name="request">the conversation parameters</param>
    /// <returns>ConversationUsageListResponse containing the usage information</returns>
    public Task<AssistantConversationUsageListResponse> QueryConversationUsageAsync(AssistantConversationParameters request)
    {
        return _client.ExecuteRequest<AssistantConversationUsageListStatus, AssistantConversationParameters, AssistantConversationParameters, AssistantConversationUsageListResponse>(
            request,
            _assistantApi.QueryConversationUsage,
            typeof(AssistantConversationUsageListResponse)
        );
    }
}