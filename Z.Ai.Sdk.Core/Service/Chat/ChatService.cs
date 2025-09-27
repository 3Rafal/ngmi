using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Service.Chat;

public class ChatService : IChatService
{
    private readonly AbstractAiClient _client;

    public ChatService(AbstractAiClient client)
    {
        _client = client;
    }

    // Chat methods will be implemented here
}