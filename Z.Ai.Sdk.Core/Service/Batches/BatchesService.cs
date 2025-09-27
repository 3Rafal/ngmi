using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Service.Batches;

public class BatchesService : IBatchesService
{
    private readonly AbstractAiClient _client;

    public BatchesService(AbstractAiClient client)
    {
        _client = client;
    }

    // Batches methods will be implemented here
}