using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Service.WebSearch;

public class WebSearchService : IWebSearchService
{
    private readonly AbstractAiClient _client;

    public WebSearchService(AbstractAiClient client)
    {
        _client = client;
    }

    // Web search methods will be implemented here
}