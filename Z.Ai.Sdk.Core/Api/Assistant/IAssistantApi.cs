using Refit;
using Z.Ai.Sdk.Core.Service.Assistant;
using Z.Ai.Sdk.Core.Service.Assistant.Conversation;
using Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;

namespace Z.Ai.Sdk.Core.Api.Assistant;

/// <summary>
/// Assistant API for intelligent conversational AI Provides advanced assistant
/// capabilities with streaming and synchronous responses Supports conversation management
/// and usage tracking for AI assistants
/// </summary>
public interface IAssistantApi
{
    /// <summary>
    /// Generate assistant response with streaming output Creates real-time conversational
    /// responses with streaming delivery
    /// </summary>
    /// <param name="request">Assistant parameters including messages, model settings, and context</param>
    /// <returns>Streaming response body with assistant's reply</returns>
    [Post("/assistant")]
    Task<Stream> AssistantCompletionStream([Body] AssistantParameters request);

    /// <summary>
    /// Generate assistant response with complete output Creates conversational responses
    /// and returns the complete assistant reply
    /// </summary>
    /// <param name="request">Assistant parameters including conversation context and settings</param>
    /// <returns>Complete assistant response with message content and metadata</returns>
    [Post("/assistant")]
    Task<AssistantCompletion> AssistantCompletion([Body] AssistantParameters request);

    /// <summary>
    /// Query assistant support capabilities Retrieves information about available
    /// assistant features and supported operations
    /// </summary>
    /// <param name="request">Query parameters for support information</param>
    /// <returns>Assistant support status and available capabilities</returns>
    [Post("/assistant/list")]
    Task<AssistantSupportStatus> QuerySupport([Body] AssistantQuerySupportParams request);

    /// <summary>
    /// Query conversation usage statistics Retrieves usage metrics and conversation
    /// history for assistant interactions
    /// </summary>
    /// <param name="request">Conversation query parameters including filters and pagination</param>
    /// <returns>Conversation usage statistics and history information</returns>
    [Post("/assistant/conversation/list")]
    Task<AssistantConversationUsageListStatus> QueryConversationUsage([Body] AssistantConversationParameters request);
}