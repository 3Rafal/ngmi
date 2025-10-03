using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Represents the result of a file deletion operation. Contains information about whether
/// the file was successfully deleted.
/// </summary>
public class FileDeleted
{
    /// <summary>
    /// The ID of the deleted file.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Indicates whether the file was successfully deleted.
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// The object type, always "file".
    /// </summary>
    public string Object { get; set; } = "file";

    /// <summary>
    /// Error information if the deletion failed.
    /// </summary>
    public ChatError? Error { get; set; }
}