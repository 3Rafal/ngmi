namespace Z.Ai.Sdk.Core.Models;

/// <summary>
/// Base interface for client responses containing response data, status information,
/// and error details for API operations.
/// </summary>
/// <typeparam name="T">Type of the response data</typeparam>
public interface IClientResponse<T>
{
    /// <summary>
    /// Gets or sets the response data
    /// </summary>
    T? Data { get; set; }

    /// <summary>
    /// Gets or sets the response status code
    /// </summary>
    int Code { get; set; }

    /// <summary>
    /// Gets or sets the response message
    /// </summary>
    string? Msg { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful
    /// </summary>
    bool Success { get; set; }

    /// <summary>
    /// Gets or sets the error details if the operation failed
    /// </summary>
    ChatError? Error { get; set; }
}