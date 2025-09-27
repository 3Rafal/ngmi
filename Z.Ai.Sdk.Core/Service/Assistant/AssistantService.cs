using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Service.Assistant;

public class AssistantService : IAssistantService
{
    private readonly AbstractAiClient _client;

    public AssistantService(AbstractAiClient client)
    {
        _client = client;
    }

    // Assistant methods will be implemented here
}