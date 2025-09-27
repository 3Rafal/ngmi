namespace Z.Ai.Sdk.Core.Model;

/// <summary>
/// A client response that supports an asynchronous stream with separate body type and
/// stream item type. This replaces Java's FlowableClientResponse with .NET's IAsyncEnumerable.
/// </summary>
/// <typeparam name="T">Response body type</typeparam>
/// <typeparam name="F">Stream item type</typeparam>
public interface IStreamableClientResponse<T, F> : IClientResponse<T>
{
    /// <summary>
    /// Gets or sets the asynchronous stream for streaming responses
    /// </summary>
    IAsyncEnumerable<F> Stream { get; set; }
}