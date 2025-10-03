using System.Text.Json;
using Microsoft.Extensions.Logging;
using Z.Ai.Sdk.Core.Config;
using Z.Ai.Sdk.Core.Service.File;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Tests.Service.File;

/// <summary>
/// FileService test class for testing various functionalities of FileService and
/// FileService implementation
/// Preserves 100% of Java logic from FileServiceTest
/// </summary>
public class FileServiceTest
{
    private readonly ILogger<FileServiceTest> _logger;
    private IFileService? _fileService;

    // Request ID template
    private static readonly string RequestIdTemplate = "file-test-{0}";

    public FileServiceTest()
    {
        _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<FileServiceTest>();
    }

    private void InitializeFileService()
    {
        var zaiConfig = new ZaiConfig();
        var apiKey = zaiConfig.ApiKey;
        if (apiKey == string.Empty)
            zaiConfig.WithApiKey("id.test-api-key");

        var client = new ZaiClient(zaiConfig);
        _fileService = client.Files();
    }

    [Fact]
    public void TestFileServiceInstantiation()
    {
        // Arrange
        InitializeFileService();

        // Assert
        Assert.NotNull(_fileService);
        Assert.IsType<FileService>(_fileService);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestFileUpload()
    {
        // Arrange
        InitializeFileService();

        // Create a temporary test file
        var tempFile = CreateTempTestFile("test-upload.txt", "This is a test file for upload.");

        try
        {
            var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            var request = new FileUploadParams
            {
                FilePath = tempFile,
                Purpose = UploadFilePurpose.Agent.Value(),
                RequestId = requestId
            };

            // Execute test
            var response = await _fileService!.UploadFileAsync(request);

            // Verify results
            Assert.NotNull(response);
            Assert.True(response.Success, $"Upload failed: {response.Msg}");
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.Id);
            Assert.Equal("file", response.Data.Object);
            Assert.Equal(UploadFilePurpose.Agent.Value(), response.Data.Purpose);
            Assert.NotNull(response.Data.Filename);
            Assert.NotNull(response.Data.Bytes);
            Assert.True(response.Data.Bytes > 0);
            Assert.Null(response.Error);

            _logger.LogInformation("File upload response: {Response}", JsonSerializer.Serialize(response));
        }
        finally
        {
            // Clean up temporary file
            if (System.IO.File.Exists(tempFile))
                System.IO.File.Delete(tempFile);
        }
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestFileUploadWithExtraJson()
    {
        // Arrange
        InitializeFileService();

        // Create a temporary test file
        var tempFile = CreateTempTestFile("test-upload-extra.txt", "This is a test file with extra parameters.");

        try
        {
            var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            // Add extra JSON parameters
            var extraJson = new Dictionary<string, JsonElement>
            {
                ["description"] = JsonSerializer.SerializeToElement("Test file with extra parameters"),
                ["version"] = JsonSerializer.SerializeToElement(1),
                ["isTest"] = JsonSerializer.SerializeToElement(true)
            };

            var request = new FileUploadParams
            {
                FilePath = tempFile,
                Purpose = UploadFilePurpose.FileExtract.Value(),
                RequestId = requestId,
                ExtraJson = extraJson
            };

            // Execute test
            var response = await _fileService!.UploadFileAsync(request);

            // Verify results
            Assert.NotNull(response);
            Assert.True(response.Success, $"Upload with extra JSON failed: {response.Msg}");
            Assert.NotNull(response.Data);
            Assert.Equal(UploadFilePurpose.FileExtract.Value(), response.Data.Purpose);
            Assert.Null(response.Error);

            _logger.LogInformation("File upload with extra JSON response: {Response}", JsonSerializer.Serialize(response));
        }
        finally
        {
            // Clean up temporary file
            if (System.IO.File.Exists(tempFile))
                System.IO.File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task TestFileUploadError_FileNotFound()
    {
        // Arrange
        InitializeFileService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new FileUploadParams
        {
            FilePath = "/non/existent/file.txt",
            Purpose = UploadFilePurpose.Agent.Value(),
            RequestId = requestId
        };

        // Execute test and expect exception
        var exception = await Assert.ThrowsAsync<FileNotFoundException>(() => _fileService!.UploadFileAsync(request));
        Assert.Contains("file not found", exception.Message);
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestListFiles()
    {
        // Arrange
        InitializeFileService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new FileListParams
        {
            Limit = 10,
            Order = "desc",
            RequestId = requestId
        };

        // Execute test
        var response = await _fileService!.ListFilesAsync(request);

        // Verify results
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Data);
        Assert.Null(response.Error);

        _logger.LogInformation("List files response: {Response}", JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestDeleteFile()
    {
        // Arrange
        InitializeFileService();
        var fileId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        var request = new FileDelRequest
        {
            FileId = fileId
        };

        // Execute test
        var response = await _fileService!.DeleteFileAsync(request);

        // Verify results
        Assert.NotNull(response);
        Assert.False(response.Success); // Should fail because file doesn't exist
        Assert.Null(response.Data);
        Assert.NotNull(response.Error);

        _logger.LogInformation("Delete file response: {Response}", JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestListFilesWithPurpose()
    {
        // Arrange
        InitializeFileService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var request = new FileListParams
        {
            Purpose = UploadFilePurpose.Agent.Value(),
            Limit = 5,
            Order = "asc",
            RequestId = requestId
        };

        // Execute test
        var response = await _fileService!.ListFilesAsync(request);

        // Verify results
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Null(response.Error);

        // Verify purpose filter if files exist
        if (response.Data.Data != null && response.Data.Data.Any())
        {
            foreach (var file in response.Data.Data)
            {
                Assert.Equal(UploadFilePurpose.Agent.Value(), file.Purpose);
            }
        }

        _logger.LogInformation("List files with purpose filter response: {Response}", JsonSerializer.Serialize(response));
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestListFilesWithPagination()
    {
        // Arrange
        InitializeFileService();
        var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        // First page
        var firstPageRequest = new FileListParams
        {
            Limit = 2,
            Order = "desc",
            RequestId = requestId
        };

        var firstPageResponse = await _fileService!.ListFilesAsync(firstPageRequest);

        Assert.NotNull(firstPageResponse);
        Assert.True(firstPageResponse.Success);

        // If there are files, test second page
        if (firstPageResponse.Data?.Data != null && firstPageResponse.Data.Data.Any())
        {
            // Get the last file ID for pagination
            var lastFileId = firstPageResponse.Data.Data.Last().Id;

            // Second page
            var secondPageRequest = new FileListParams
            {
                Limit = 2,
                Order = "desc",
                After = lastFileId,
                RequestId = requestId + "-page2"
            };

            var secondPageResponse = await _fileService!.ListFilesAsync(secondPageRequest);

            Assert.NotNull(secondPageResponse);
            Assert.True(secondPageResponse.Success);

            _logger.LogInformation("Pagination test - First page: {FirstPage}, Second page: {SecondPage}",
                JsonSerializer.Serialize(firstPageResponse), JsonSerializer.Serialize(secondPageResponse));
        }
        else
        {
            _logger.LogInformation("No files available for pagination test");
        }
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestRetrieveFileContent()
    {
        // First upload a file to get a valid file ID
        var tempFile = CreateTempTestFile("test-content.txt", "This is test content for retrieval.");

        try
        {
            InitializeFileService();
            var requestId = string.Format(RequestIdTemplate, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            var uploadRequest = new FileUploadParams
            {
                FilePath = tempFile,
                Purpose = UploadFilePurpose.FileExtract.Value(),
                RequestId = requestId
            };

            var uploadResponse = await _fileService!.UploadFileAsync(uploadRequest);
            Assert.True(uploadResponse.Success);

            var fileId = uploadResponse.Data?.Id;

            // Retrieve file content
            var content = await _fileService!.RetrieveFileContentAsync(fileId!);

            // Verify results
            Assert.NotNull(content);

            // Verify we can read content
            var contentBytes = await content.GetContentAsync();
            Assert.NotNull(contentBytes);
            Assert.True(contentBytes.Length > 0);

            _logger.LogInformation("File content retrieved successfully for file ID: {FileId}", fileId);
        }
        finally
        {
            // Clean up temporary file
            if (System.IO.File.Exists(tempFile))
                System.IO.File.Delete(tempFile);
        }
    }

    [RequiresEnvironmentVariableFact("ZAI_API_KEY")]
    public async Task TestRetrieveFileContentError_InvalidFileId()
    {
        // Arrange
        InitializeFileService();
        var invalidFileId = "invalid-file-id-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Execute test and expect IOException
        await Assert.ThrowsAsync<IOException>(() => _fileService!.RetrieveFileContentAsync(invalidFileId));
    }

    [Fact]
    public async Task TestValidation_NullUploadRequest()
    {
        // Arrange
        InitializeFileService();

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _fileService!.UploadFileAsync(null!));
    }

    [Fact]
    public async Task TestValidation_NullListRequest()
    {
        // Arrange
        InitializeFileService();

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _fileService!.ListFilesAsync(null!));
    }

    [Fact]
    public async Task TestValidation_NullFileId()
    {
        // Arrange
        InitializeFileService();

        // Execute test and expect exception
        await Assert.ThrowsAsync<ArgumentException>(() => _fileService!.RetrieveFileContentAsync(null!));
    }

    /// <summary>
    /// Helper method to create a temporary test file
    /// </summary>
    /// <param name="filename">The filename to create</param>
    /// <param name="content">The content to write to the file</param>
    /// <returns>The full path to the created temporary file</returns>
    private static string CreateTempTestFile(string filename, string content)
    {
        var tempDir = Path.GetTempPath();
        var tempFile = Path.Combine(tempDir, filename);
        System.IO.File.WriteAllText(tempFile, content);
        return tempFile;
    }
}