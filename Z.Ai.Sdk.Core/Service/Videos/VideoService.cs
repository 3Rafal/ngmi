using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Service.Videos;

public class VideoService : IVideoService
{
    private readonly AbstractAiClient _client;

    public VideoService(AbstractAiClient client)
    {
        _client = client;
    }

    // Video methods will be implemented here
}