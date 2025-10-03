using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Request to delete a file by ID
/// </summary>
public class FileDelRequest : IClientRequest<FileDelRequest>
{
    /// <summary>
    /// The ID of the file to delete
    /// </summary>
    public string? FileId { get; set; }

    /// <summary>
    /// Initializes a new instance of FileDelRequest
    /// </summary>
    public FileDelRequest() { }

    /// <summary>
    /// Initializes a new instance of FileDelRequest
    /// </summary>
    /// <param name="fileId">The ID of the file to delete</param>
    public FileDelRequest(string fileId)
    {
        FileId = fileId;
    }
}