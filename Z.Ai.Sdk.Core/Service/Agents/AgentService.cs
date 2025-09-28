using Z.Ai.Sdk.Core.Api.Agents;

namespace Z.Ai.Sdk.Core.Service.Agents;

public class AgentService : IAgentService
{
    private readonly AbstractAiClient _client;
    private readonly IAgentsApi _agentsApi;
    private readonly IAgentsApi _streamingAgentsApi;

    public AgentService(AbstractAiClient client)
    {
        _client = client;
        _agentsApi = client.CreateRefitService<IAgentsApi>();
        _streamingAgentsApi = client.CreateRefitStreamingService<IAgentsApi>();
    }

    public Task<Model.ChatCompletionResponse> CreateAgentCompletionAsync(AgentsCompletionRequest request)
    {
        ValidateParams(request);
        return request.Stream == true 
            ? StreamAgentCompletionAsync(request) 
            : SyncAgentCompletionAsync(request);
    }

    public Task<Model.ModelData> RetrieveAgentAsyncResultAsync(AgentAsyncResultRetrieveParams request)
    {
        return _agentsApi.QueryAgentsAsyncResult(request);
    }

    private Task<Model.ChatCompletionResponse> StreamAgentCompletionAsync(AgentsCompletionRequest request)
    {
        return _client.BiStreamRequest<Model.ModelData, Model.ModelData, AgentsCompletionRequest, AgentsCompletionRequest, Model.ChatCompletionResponse>(
            request,
            req => _streamingAgentsApi.AgentsCompletionStream(req),
            typeof(Model.ChatCompletionResponse),
            typeof(Model.ModelData)
        );
    }

    private Task<Model.ChatCompletionResponse> SyncAgentCompletionAsync(AgentsCompletionRequest request)
    {
        return _client.ExecuteRequest<Model.ModelData, AgentsCompletionRequest, AgentsCompletionRequest, Model.ChatCompletionResponse>(
            request,
            req => _agentsApi.AgentsCompletionSync(req),
            typeof(Model.ChatCompletionResponse)
        );
    }

    private static void ValidateParams(AgentsCompletionRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "request cannot be null");
        }
        if (request.Messages == null || request.Messages.Count == 0)
        {
            throw new ArgumentException("request messages cannot be null or empty", nameof(request.Messages));
        }
        if (string.IsNullOrEmpty(request.AgentId))
        {
            throw new ArgumentException("request agent_id cannot be null", nameof(request.AgentId));
        }
    }
}