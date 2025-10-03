using System.Net.Http.Headers;
using System.Text.Json;
using Z.Ai.Sdk.Core.Api.File;
using Z.Ai.Sdk.Core.Model;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.File;

/// <summary>
/// File service implementation for managing file operations
/// Preserves 100% of Java logic from FileServiceImpl
/// </summary>
public class FileService : IFileService
{
    private readonly AbstractAiClient _client;
    private readonly IFileApi _fileApi;

    /// <summary>
    /// Initializes a new instance of FileService
    /// </summary>
    /// <param name="client">The AI client instance</param>
    public FileService(AbstractAiClient client)
    {
        _client = client;
        _fileApi = client.CreateRefitService<IFileApi>();
    }

    /// <summary>
    /// Uploads a file to the server.
    /// </summary>
    /// <param name="request">The file upload request</param>
    /// <returns>FileApiResponse containing the upload result</returns>
    public async Task<FileApiResponse> UploadFileAsync(FileUploadParams request)
    {
        if (request == null)
        {
            throw new ArgumentException("request cannot be null", nameof(request));
        }
        if (string.IsNullOrEmpty(request.FilePath))
        {
            throw new ArgumentException("request path cannot be null", nameof(request.FilePath));
        }

        var fileInfo = new System.IO.FileInfo(request.FilePath);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException("file not found", request.FilePath);
        }

        try
        {
            using var multipartContent = new MultipartFormDataContent();

            // Add file content
            var fileContent = new StreamContent(System.IO.File.OpenRead(request.FilePath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            multipartContent.Add(fileContent, "file", fileInfo.Name);

            // Add purpose
            multipartContent.Add(new StringContent(request.Purpose ?? string.Empty), "purpose");

            // Add extra metadata if present
            if (request.ExtraJson != null)
            {
                foreach (var kvp in request.ExtraJson)
                {
                    if (kvp.Value is JsonElement element)
                    {
                        switch (element.ValueKind)
                        {
                            case JsonValueKind.String:
                                multipartContent.Add(new StringContent(element.GetString() ?? string.Empty), kvp.Key);
                                break;
                            case JsonValueKind.Number:
                                multipartContent.Add(new StringContent(element.GetRawText()), kvp.Key);
                                break;
                            case JsonValueKind.True:
                            case JsonValueKind.False:
                                multipartContent.Add(new StringContent(element.GetBoolean().ToString()), kvp.Key);
                                break;
                            default:
                                multipartContent.Add(new StringContent(element.ToString()), kvp.Key);
                                break;
                        }
                    }
                    else
                    {
                        multipartContent.Add(new StringContent(kvp.Value.ToString() ?? string.Empty), kvp.Key);
                    }
                }
            }

            var apiResponse = await _fileApi.UploadFile(multipartContent);
            return new FileApiResponse
            {
                Code = 200,
                Msg = "File uploaded successfully",
                Success = true,
                Data = apiResponse
            };
        }
        catch (Exception ex)
        {
            return new FileApiResponse
            {
                Code = 500,
                Msg = $"File upload failed: {ex.Message}",
                Success = false,
                Error = new ChatError(
                    Code: 500,
                    Message: ex.Message
                )
            };
        }
    }

    /// <summary>
    /// Delete file by file ID.
    /// </summary>
    /// <param name="request">The file delete request containing the file ID</param>
    /// <returns>FileDelResponse containing the delete result</returns>
    public async Task<FileDelResponse> DeleteFileAsync(FileDelRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.FileId))
        {
            throw new ArgumentException("request and fileId cannot be null", nameof(request));
        }

        try
        {
            var deletedFile = await _fileApi.DeleteFile(request.FileId);
            return new FileDelResponse
            {
                Code = 200,
                Msg = "File deleted successfully",
                Success = true,
                Data = deletedFile
            };
        }
        catch (Exception ex)
        {
            return new FileDelResponse
            {
                Code = 500,
                Msg = $"File deletion failed: {ex.Message}",
                Success = false,
                Error = new ChatError(
                    Code: 500,
                    Message: ex.Message
                )
            };
        }
    }

    /// <summary>
    /// Lists all files.
    /// </summary>
    /// <param name="queryFilesRequest">FileListParams containing the query parameters for listing files</param>
    /// <returns>QueryFileApiResponse containing the list of files</returns>
    public async Task<QueryFileApiResponse> ListFilesAsync(FileListParams queryFilesRequest)
    {
        if (queryFilesRequest == null)
        {
            throw new ArgumentException("queryFilesRequest cannot be null", nameof(queryFilesRequest));
        }

        try
        {
            var queryResult = await _fileApi.QueryFileList(
                queryFilesRequest.After,
                queryFilesRequest.Purpose,
                queryFilesRequest.Order,
                queryFilesRequest.Limit);

            return new QueryFileApiResponse
            {
                Code = 200,
                Msg = "Files retrieved successfully",
                Success = true,
                Data = queryResult
            };
        }
        catch (Exception ex)
        {
            return new QueryFileApiResponse
            {
                Code = 500,
                Msg = $"File list retrieval failed: {ex.Message}",
                Success = false,
                Error = new ChatError(
                    Code: 500,
                    Message: ex.Message
                )
            };
        }
    }

    /// <summary>
    /// Retrieves the content of a specific file.
    /// </summary>
    /// <param name="fileId">The ID of the file to retrieve</param>
    /// <returns>HttpxBinaryResponseContent containing the file content</returns>
    public async Task<HttpxBinaryResponseContent> RetrieveFileContentAsync(string fileId)
    {
        if (string.IsNullOrEmpty(fileId))
        {
            throw new ArgumentException("fileId cannot be null or empty", nameof(fileId));
        }

        var response = await _fileApi.GetFileContent(fileId);

        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            throw new IOException("Failed to get the file content");
        }

        return new HttpxBinaryResponseContent(response);
    }
}