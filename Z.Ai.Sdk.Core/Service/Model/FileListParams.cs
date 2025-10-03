using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Query file list parameters
/// </summary>
public record FileListParams(
    /// <summary>
    /// Filter files by their intended purpose (e.g., fine-tune, assistants)
    /// </summary>
    string? Purpose = null,

    /// <summary>
    /// Maximum number of files to return per page
    /// </summary>
    int? Limit = null,

    /// <summary>
    /// Cursor for pagination to get files after this point
    /// </summary>
    string? After = null,

    /// <summary>
    /// Sort order for the file list (e.g., created_at)
    /// </summary>
    string? Order = null
) : CommonRequestBase, IClientRequest<FileListParams>;