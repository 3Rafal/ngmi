namespace Z.Ai.Sdk.Core.Model;

/// <summary>
/// Represents an error that occurred during API operations. Contains error code and
/// descriptive message for troubleshooting.
/// </summary>
/// <param name="Code">Error code indicating the type of error</param>
/// <param name="Message">Descriptive error message</param>
public record ChatError(
    int? Code = null,
    string? Message = null
);