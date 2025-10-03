using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Agents;

public interface IAgentService
{

    /// <summary>
    /// Creates an agent completion, either streaming or non-streaming based on the request configuration.
    /// </summary>
    /// <param name="request">The agents completion request.</param>
    /// <returns>ChatCompletionResponse containing the agent completion result.</returns>
    Task<ChatCompletionResponse> CreateAgentCompletionAsync(AgentsCompletionRequest request);
}