using System.IO;
using System.Threading.Tasks;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.File;

/// <summary>
/// File service interface for managing file operations
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Uploads a file to the server.
    /// </summary>
    /// <param name="request">The file upload request</param>
    /// <returns>FileApiResponse containing the upload result</returns>
    Task<FileApiResponse> UploadFileAsync(FileUploadParams request);

    /// <summary>
    /// Delete file by file ID.
    /// </summary>
    /// <param name="request">The file delete request containing the file ID</param>
    /// <returns>FileDelResponse containing the delete result</returns>
    Task<FileDelResponse> DeleteFileAsync(FileDelRequest request);

    /// <summary>
    /// Lists all files.
    /// </summary>
    /// <param name="queryFilesRequest">FileListParams containing the query parameters for listing files</param>
    /// <returns>QueryFileApiResponse containing the list of files</returns>
    Task<QueryFileApiResponse> ListFilesAsync(FileListParams queryFilesRequest);

    /// <summary>
    /// Retrieves the content of a specific file.
    /// </summary>
    /// <param name="fileId">The ID of the file to retrieve</param>
    /// <returns>HttpxBinaryResponseContent containing the file content</returns>
    Task<HttpxBinaryResponseContent> RetrieveFileContentAsync(string fileId);
}