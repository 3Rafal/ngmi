using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Result of a file list query operation
/// </summary>
public class QueryFileResult
{
    /// <summary>
    /// The type of object returned, should be "list".
    /// </summary>
    public string? Object { get; set; }

    /// <summary>
    /// List of files matching the query criteria
    /// </summary>
    public List<FileInfo>? Data { get; set; }

    /// <summary>
    /// Error information if the query failed
    /// </summary>
    public ChatError? Error { get; set; }
}