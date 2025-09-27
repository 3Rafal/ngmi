using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Services.Audio;

public class AudioService : IAudioService
{
    private readonly AbstractAiClient _client;

    public AudioService(AbstractAiClient client)
    {
        _client = client;
    }

    // Audio methods will be implemented here
}