using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Services.VoiceClone;

public class VoiceCloneService : IVoiceCloneService
{
    private readonly AbstractAiClient _client;

    public VoiceCloneService(AbstractAiClient client)
    {
        _client = client;
    }

    // Voice clone methods will be implemented here
}