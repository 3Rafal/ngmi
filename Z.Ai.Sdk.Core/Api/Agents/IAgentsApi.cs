using Refit;
using Z.Ai.Sdk.Core.Service.Agents;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Api.Agents;

/// <summary>
/// Agents API for intelligent agent capabilities based on GLM-4 All Tools Provides AI
/// agents with automatic tool calling, function execution, and complex task automation
/// Features include web browsing, code interpreter, image generation, and multi-tool
/// coordination Supports streaming and synchronous agent interactions with real-time task
/// planning
/// </summary>
public interface IAgentsApi
{
    /// <summary>
    /// Create a streaming agent completion with GLM-4 All Tools Returns agent responses in
    /// real-time through Server-Sent Events (SSE) Automatically selects and calls
    /// appropriate tools (web search, code execution, image generation)
    /// </summary>
    /// <param name="request">Agent completion parameters including tools, functions, and
    /// execution context</param>
    /// <returns>Streaming response body with incremental agent outputs and tool execution
    /// results</returns>
    [Post("/v1/agents")]
    [Headers("Accept: text/event-stream")]
    Task<Stream> AgentsCompletionStream([Body] AgentsCompletionRequest request);

    /// <summary>
    /// Create a synchronous agent completion with GLM-4 All Tools Waits for the agent to
    /// complete complex task execution and returns the final result Supports automatic
    /// tool selection including CogView3 image generation, Python code interpreter, and
    /// web browsing
    /// </summary>
    /// <param name="request">Agent completion parameters including tools, functions, and
    /// execution context</param>
    /// <returns>Complete agent execution response with results and comprehensive tool
    /// outputs</returns>
    [Post("/v1/agents")]
    Task<ModelData> AgentsCompletionSync([Body] AgentsCompletionRequest request);

    /// <summary>
    /// Query the result of an asynchronous agent execution Retrieves the agent execution
    /// result using the task parameters for long-running tasks Useful for complex
    /// multi-tool operations that require extended processing time
    /// </summary>
    /// <param name="request">Parameters for retrieving asynchronous agent execution results</param>
    /// <returns>Agent execution result with tool outputs, status information, and task
    /// completion details</returns>
    [Post("/v1/agents/async-result")]
    Task<ModelData> QueryAgentsAsyncResult([Body] AgentAsyncResultRetrieveParams request);
}
