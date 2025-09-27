using Z.Ai.Sdk.Core;

namespace Z.Ai.Sdk.Core.Services.File;

public class FileService : IFileService
{
    private readonly AbstractAiClient _client;

    public FileService(AbstractAiClient client)
    {
        _client = client;
    }

    // File methods will be implemented here
}