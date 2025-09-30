using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;

/// <summary>
/// Response for assistant support query API calls.
/// </summary>
public class AssistantSupportResponse : IClientResponse<AssistantSupportStatus>
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
    /// Indicates whether the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The assistant support data.
    /// </summary>
    public AssistantSupportStatus? Data { get; set; }

    /// <summary>
    /// Error information if the request failed.
    /// </summary>
    public ChatError? Error { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public AssistantSupportResponse() { }

    /// <summary>
    /// Constructor with code and message.
    /// </summary>
    /// <param name="code">the response code</param>
    /// <param name="msg">the response message</param>
    public AssistantSupportResponse(int code, string msg)
    {
        Code = code;
        Msg = msg;
    }
}