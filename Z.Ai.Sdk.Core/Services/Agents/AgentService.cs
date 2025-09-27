using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Services.Agents;

public class AgentService : IAgentService
{
    private readonly AbstractAiClient _client;

    public AgentService(AbstractAiClient client)
    {
        _client = client;
    }

    // Agent completion methods will be implemented here
}