using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Services.Embedding;

public class EmbeddingService : IEmbeddingService
{
    private readonly AbstractAiClient _client;

    public EmbeddingService(AbstractAiClient client)
    {
        _client = client;
    }

    // Embedding methods will be implemented here
}