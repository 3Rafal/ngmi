using System.Text;
using System.IO;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Wrapper for HTTP binary response content, equivalent to Java HttpxBinaryResponseContent
/// </summary>
public class HttpxBinaryResponseContent : IDisposable
{
    private readonly HttpResponseMessage _response;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of HttpxBinaryResponseContent
    /// </summary>
    /// <param name="response">The HTTP response message</param>
    public HttpxBinaryResponseContent(HttpResponseMessage response)
    {
        _response = response ?? throw new ArgumentException("Response cannot be null");
        // Stream will be accessed on demand from response content
    }

    /// <summary>
    /// Gets the content as a byte array
    /// </summary>
    /// <returns>Content as byte array</returns>
    public async Task<byte[]> GetContentAsync()
    {
        // For HTTP response streams, read the response content directly
        return await _response.Content.ReadAsByteArrayAsync();
    }

    /// <summary>
    /// Gets the content as text
    /// </summary>
    /// <returns>Content as string</returns>
    public async Task<string> GetTextAsync()
    {
        return await _response.Content.ReadAsStringAsync();
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
        var allBytes = await GetContentAsync();
        var offset = 0;

        while (offset < allBytes.Length)
        {
            var remainingBytes = allBytes.Length - offset;
            var currentChunkSize = Math.Min(chunkSize, remainingBytes);
            var chunk = new byte[currentChunkSize];
            Array.Copy(allBytes, offset, chunk, 0, currentChunkSize);

            yield return chunk;
            offset += currentChunkSize;
        }
    }

    /// <summary>
    /// Gets an iterator for reading content in text chunks
    /// </summary>
    /// <param name="chunkSize">Size of each chunk in bytes</param>
    /// <returns>Iterator of strings</returns>
    public async IAsyncEnumerable<string> IterTextAsync(int chunkSize)
    {
        var allBytes = await GetContentAsync();
        var allText = Encoding.UTF8.GetString(allBytes);
        var offset = 0;

        while (offset < allText.Length)
        {
            var remainingChars = allText.Length - offset;
            var currentChunkSize = Math.Min(chunkSize, remainingChars);

            yield return allText.Substring(offset, currentChunkSize);
            offset += currentChunkSize;
        }
    }

    /// <summary>
    /// Writes the content to a file
    /// </summary>
    /// <param name="filePath">Path to the output file</param>
    public async Task WriteToFileAsync(string filePath)
    {
        var contentBytes = await GetContentAsync();
        await System.IO.File.WriteAllBytesAsync(filePath, contentBytes);
    }

    /// <summary>
    /// Streams the content to a file with specific chunk size
    /// </summary>
    /// <param name="filePath">Path to the output file</param>
    /// <param name="chunkSize">Size of each chunk</param>
    public async Task StreamToFileAsync(string filePath, int chunkSize)
    {
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        await foreach (var chunk in IterBytesAsync(chunkSize))
        {
            await fileStream.WriteAsync(chunk, 0, chunk.Length);
        }
    }

    /// <summary>
    /// Disposes the response content and underlying resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _response?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}