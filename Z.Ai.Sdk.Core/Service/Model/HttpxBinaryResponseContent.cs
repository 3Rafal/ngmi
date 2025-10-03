using System.Text;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Wrapper for HTTP binary response content, equivalent to Java HttpxBinaryResponseContent
/// </summary>
public class HttpxBinaryResponseContent : IDisposable
{
    private readonly HttpResponseMessage _response;
    private readonly Stream _contentStream;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of HttpxBinaryResponseContent
    /// </summary>
    /// <param name="response">The HTTP response message</param>
    public HttpxBinaryResponseContent(HttpResponseMessage response)
    {
        _response = response ?? throw new ArgumentException("Response cannot be null");
        _contentStream = response.Content.ReadAsStream();

        if (_contentStream == null)
        {
            throw new InvalidOperationException("Response content stream cannot be null");
        }
    }

    /// <summary>
    /// Gets the content as a byte array
    /// </summary>
    /// <returns>Content as byte array</returns>
    public async Task<byte[]> GetContentAsync()
    {
        _contentStream.Seek(0, SeekOrigin.Begin);
        using var memoryStream = new MemoryStream();
        await _contentStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Gets the content as text
    /// </summary>
    /// <returns>Content as string</returns>
    public async Task<string> GetTextAsync()
    {
        _contentStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(_contentStream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Gets the content encoding from the response headers
    /// </summary>
    /// <returns>Content encoding string</returns>
    public string? GetEncoding()
    {
        var contentType = _response.Content.Headers.ContentType?.CharSet;
        return contentType;
    }

    /// <summary>
    /// Gets an iterator for reading content in byte chunks
    /// </summary>
    /// <param name="chunkSize">Size of each chunk</param>
    /// <returns>Iterator of byte arrays</returns>
    public async IAsyncEnumerable<byte[]> IterBytesAsync(int chunkSize)
    {
        _contentStream.Seek(0, SeekOrigin.Begin);
        var buffer = new byte[chunkSize];

        while (true)
        {
            int bytesRead = await _contentStream.ReadAsync(buffer, 0, chunkSize);
            if (bytesRead == 0)
                yield break;

            var chunk = new byte[bytesRead];
            Array.Copy(buffer, chunk, bytesRead);
            yield return chunk;
        }
    }

    /// <summary>
    /// Gets an iterator for reading content in text chunks
    /// </summary>
    /// <param name="chunkSize">Size of each chunk in bytes</param>
    /// <returns>Iterator of strings</returns>
    public async IAsyncEnumerable<string> IterTextAsync(int chunkSize)
    {
        _contentStream.Seek(0, SeekOrigin.Begin);
        var buffer = new byte[chunkSize];

        while (true)
        {
            int bytesRead = await _contentStream.ReadAsync(buffer, 0, chunkSize);
            if (bytesRead == 0)
                yield break;

            yield return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }

    /// <summary>
    /// Writes the content to a file
    /// </summary>
    /// <param name="filePath">Path to the output file</param>
    public async Task WriteToFileAsync(string filePath)
    {
        _contentStream.Seek(0, SeekOrigin.Begin);
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await _contentStream.CopyToAsync(fileStream);
    }

    /// <summary>
    /// Streams the content to a file with specific chunk size
    /// </summary>
    /// <param name="filePath">Path to the output file</param>
    /// <param name="chunkSize">Size of each chunk</param>
    public async Task StreamToFileAsync(string filePath, int chunkSize)
    {
        _contentStream.Seek(0, SeekOrigin.Begin);
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        var buffer = new byte[chunkSize];

        while (true)
        {
            int bytesRead = await _contentStream.ReadAsync(buffer, 0, chunkSize);
            if (bytesRead == 0)
                break;

            await fileStream.WriteAsync(buffer, 0, bytesRead);
        }
    }

    /// <summary>
    /// Disposes the response content and underlying resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _contentStream?.Dispose();
            _response?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}