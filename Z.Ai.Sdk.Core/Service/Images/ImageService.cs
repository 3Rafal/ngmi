using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Service.Images;

public class ImageService : IImageService
{
    private readonly AbstractAiClient _client;

    public ImageService(AbstractAiClient client)
    {
        _client = client;
    }

    // Image methods will be implemented here
}