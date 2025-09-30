using System.Collections.Generic;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// Response wrapper for Assistant API calls that supports both synchronous and streaming
/// responses. This class implements IStreamableClientResponse to handle streaming assistant
/// completions.
/// </summary>
public class AssistantApiResponse : IStreamableClientResponse<AssistantCompletion, AssistantCompletion>
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
    /// The assistant completion data for synchronous responses.
    /// </summary>
    public AssistantCompletion? Data { get; set; }

    /// <summary>
    /// Error information if the request failed.
    /// </summary>
    public ChatError? Error { get; set; }

    /// <summary>
    /// The stream for streaming responses.
    /// </summary>
    public IAsyncEnumerable<AssistantCompletion> Stream { get; set; } = System.Collections.Immutable.ImmutableArray<AssistantCompletion>.Empty.ToAsyncEnumerable();

    /// <summary>
    /// Default constructor.
    /// </summary>
    public AssistantApiResponse() { }

    /// <summary>
    /// Constructor with code and message.
    /// </summary>
    /// <param name="code">the response code</param>
    /// <param name="msg">the response message</param>
    public AssistantApiResponse(int code, string msg)
    {
        Code = code;
        Msg = msg;
    }
}