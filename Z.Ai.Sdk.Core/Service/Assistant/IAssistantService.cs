using Z.Ai.Sdk.Core.Service.Assistant.Conversation;
using Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// Assistant service interface
/// </summary>
public interface IAssistantService
{
    /// <summary>
    /// Creates a streaming assistant completion.
    /// </summary>
    /// <param name="request">the assistant completion request</param>
    /// <returns>AssistantApiResponse containing the completion result</returns>
    Task<AssistantApiResponse> AssistantCompletionStreamAsync(AssistantParameters request);

    /// <summary>
    /// Creates a non-streaming assistant completion.
    /// </summary>
    /// <param name="request">the assistant completion request</param>
    /// <returns>AssistantApiResponse containing the completion result</returns>
    Task<AssistantApiResponse> AssistantCompletionAsync(AssistantParameters request);

    /// <summary>
    /// Queries assistant support status.
    /// </summary>
    /// <param name="request">the query support request</param>
    /// <returns>AssistantSupportResponse containing the support information</returns>
    Task<AssistantSupportResponse> QuerySupportAsync(AssistantQuerySupportParams request);

    /// <summary>
    /// Queries conversation usage information.
    /// </summary>
    /// <param name="request">the conversation parameters</param>
    /// <returns>ConversationUsageListResponse containing the usage information</returns>
    Task<AssistantConversationUsageListResponse> QueryConversationUsageAsync(AssistantConversationParameters request);
}