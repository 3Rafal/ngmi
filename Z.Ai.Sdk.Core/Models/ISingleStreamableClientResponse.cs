namespace Z.Ai.Sdk.Core.Models;

/// <summary>
/// Simplified client response with an asynchronous stream where the body type and stream item
/// type are the same. This replaces Java's FlowableClientResponse with a more .NET-idiomatic approach.
/// </summary>
/// <typeparam name="T">Response data and stream item type</typeparam>
public interface ISingleStreamableClientResponse<T> : IStreamableClientResponse<T, T>
{
    // No additional methods needed - inherits all functionality from IStreamableClientResponse<T, T>
}