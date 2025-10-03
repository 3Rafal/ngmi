using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// A file uploaded to ZAi
/// </summary>
public class FileInfo
{
    /// <summary>
    /// The unique id of this file.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The type of object returned, should be "file".
    /// </summary>
    public string? Object { get; set; }

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long? Bytes { get; set; }

    /// <summary>
    /// The creation time in epoch seconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long? CreatedAt { get; set; }

    /// <summary>
    /// The name of the file.
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Description of the file's purpose.
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// The current status of the file, which can be either uploaded, processed, pending,
    /// error, deleting or deleted.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Additional details about the status of the file. If the file is in the error state,
    /// this will include a message describing the error.
    /// </summary>
    [JsonPropertyName("status_details")]
    public string? StatusDetails { get; set; }

    /// <summary>
    /// Error information if the file operation failed.
    /// </summary>
    public ChatError? Error { get; set; }
}