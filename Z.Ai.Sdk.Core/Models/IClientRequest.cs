namespace Z.Ai.Sdk.Core.Models;

/// <summary>
/// Marker interface for client requests. Can be extended to provide
/// common request functionality across different service implementations.
/// </summary>
/// <typeparam name="T">Type of the request data</typeparam>
public interface IClientRequest<T>
{
}