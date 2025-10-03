using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Response wrapper for file list query operations
/// </summary>
public class QueryFileApiResponse : IClientResponse<QueryFileResult>
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
    /// File list query result information
    /// </summary>
    public QueryFileResult? Data { get; set; }

    /// <summary>
    /// Error information if the operation failed
    /// </summary>
    public ChatError? Error { get; set; }
}