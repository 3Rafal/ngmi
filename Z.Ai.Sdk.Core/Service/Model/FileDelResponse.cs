using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Response wrapper for file deletion operations
/// </summary>
public class FileDelResponse : IClientResponse<FileDeleted>
{
    /// <summary>
    /// Response code
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Response message
    /// </summary>
    public string? Msg { get; set; }

    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// File deletion result information
    /// </summary>
    public FileDeleted? Data { get; set; }

    /// <summary>
    /// Error information if the operation failed
    /// </summary>
    public ChatError? Error { get; set; }
}