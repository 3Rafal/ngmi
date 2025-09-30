using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Assistant.Conversation;

/// <summary>
/// Response for conversation usage list API calls. This class contains the response data
/// for retrieving conversation usage statistics.
/// </summary>
public class AssistantConversationUsageListResponse : IClientResponse<AssistantConversationUsageListStatus>
{
    /// <summary>
    /// Response status code.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Response message.
    /// </summary>
    public string? Msg { get; set; }

    /// <summary>
    /// Indicates if the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The conversation usage list data.
    /// </summary>
    public AssistantConversationUsageListStatus? Data { get; set; }

    /// <summary>
    /// Error information if the request failed.
    /// </summary>
    public ChatError? Error { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public AssistantConversationUsageListResponse() { }

    /// <summary>
    /// Constructor with code and message.
    /// </summary>
    /// <param name="code">the response code</param>
    /// <param name="msg">the response message</param>
    public AssistantConversationUsageListResponse(int code, string msg)
    {
        Code = code;
        Msg = msg;
    }
}