using Z.Ai.Sdk.Core.Service.Model;
using Z.Ai.Sdk.Core.Service.Agents;

namespace Z.Ai.Sdk.Core.Service.Agents;

public interface IAgentService
{
    
    /// <summary>
    /// Creates an agent completion, either streaming or non-streaming based on the request configuration.
    /// </summary>
    /// <param name="request">The agents completion request.</param>
    /// <returns>ChatCompletionResponse containing the agent completion result.</returns>
    Task<Model.ChatCompletionResponse> CreateAgentCompletionAsync(AgentsCompletionRequest request);

    /// <summary>
    /// Retrieves the result of an asynchronous agent operation.
    /// </summary>
    /// <param name="request">The query request for the async agent result.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the async agent operation result.</returns>
    Task<Model.ModelData> RetrieveAgentAsyncResultAsync(AgentAsyncResultRetrieveParams request);

}