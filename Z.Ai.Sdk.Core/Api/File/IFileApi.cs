using Refit;
using Z.Ai.Sdk.Core.Service.Model;
using ZAiFileInfo = Z.Ai.Sdk.Core.Service.Model.FileInfo;

namespace Z.Ai.Sdk.Core.Api.File;

/// <summary>
/// File Management API for document and data handling
/// Provides file upload, retrieval, deletion, and content access capabilities
/// Supports various file formats for fine-tuning, knowledge base, and other AI tasks
/// </summary>
public interface IFileApi
{
    /// <summary>
    /// Upload a file to the platform
    /// Stores files for use in fine-tuning, knowledge base, or other AI operations
    /// </summary>
    /// <param name="multipartContent">File data with metadata including purpose and format</param>
    /// <returns>File information including ID, name, size, and upload status</returns>
    [Post("/files")]
    Task<ZAiFileInfo> UploadFile(MultipartFormDataContent multipartContent);

    /// <summary>
    /// Delete a file from the platform
    /// Permanently removes the file and all associated data
    /// </summary>
    /// <param name="fileId">Unique identifier of the file to delete</param>
    /// <returns>Confirmation of file deletion with status information</returns>
    [Delete("/files/{fileId}")]
    Task<FileDeleted> DeleteFile(string fileId);

    /// <summary>
    /// Query and list files with filtering options
    /// Retrieves a paginated list of files with optional filtering by purpose and ordering
    /// </summary>
    /// <param name="after">Cursor for pagination to get files after this point</param>
    /// <param name="purpose">Filter files by their intended purpose (e.g., fine-tune, assistants)</param>
    /// <param name="order">Sort order for the file list (e.g., created_at)</param>
    /// <param name="limit">Maximum number of files to return per page</param>
    /// <returns>Paginated list of files with metadata</returns>
    [Get("/files")]
    Task<QueryFileResult> QueryFileList(
        [Query] string? after = null,
        [Query] string? purpose = null,
        [Query] string? order = null,
        [Query] int? limit = null);

    /// <summary>
    /// Download file content
    /// Streams the actual file content for download or processing
    /// </summary>
    /// <param name="fileId">Unique identifier of the file to download</param>
    /// <returns>Streaming file content in original format</returns>
    [Headers("Accept: application/octet-stream")]
    [Get("/files/{fileId}/content")]
    Task<HttpResponseMessage> GetFileContent(string fileId);
}