using System;

namespace Z.Ai.Sdk.Core;

/// <summary>
/// Exception class for Z.AI API errors that includes error code, message, and HTTP status.
/// </summary>
public class ZaiHttpException : Exception
{
    /// <summary>
    /// Gets the HTTP status code of the error.
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Gets the error code from the API response.
    /// </summary>
    public string? Code { get; }

    /// <summary>
    /// Initializes a new instance of the ZaiHttpException class.
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="code">The error code</param>
    /// <param name="innerException">The inner exception</param>
    public ZaiHttpException(string message, string? code, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
        StatusCode = null;
    }

    /// <summary>
    /// Initializes a new instance of the ZaiHttpException class with HTTP status code.
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="code">The error code</param>
    /// <param name="statusCode">The HTTP status code</param>
    /// <param name="innerException">The inner exception</param>
    public ZaiHttpException(string message, string? code, int? statusCode, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
        StatusCode = statusCode;
    }
}