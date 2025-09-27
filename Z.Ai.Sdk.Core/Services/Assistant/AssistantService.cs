using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Services.Assistant;

public class AssistantService : IAssistantService
{
    private readonly AbstractAiClient _client;

    public AssistantService(AbstractAiClient client)
    {
        _client = client;
    }

    // Assistant methods will be implemented here
}