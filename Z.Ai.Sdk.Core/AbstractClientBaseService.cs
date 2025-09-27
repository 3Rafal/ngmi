using System;
using System.Text.Json;
using Z.Ai.Sdk.Core.Model;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core;

/// <summary>
/// Abstract base service providing common HTTP request execution functionality
/// for all API client implementations. This class handles request execution,
/// response processing, and error handling for both synchronous and streaming API calls.
/// </summary>
public abstract class AbstractClientBaseService
{
    /// <summary>
    /// Executes a synchronous API request.
    /// </summary>
    /// <typeparam name="TData">The type of data returned by the API</typeparam>
    /// <typeparam name="TParam">The type of parameters sent to the API</typeparam>
    /// <typeparam name="TRequest">The type of the request object</typeparam>
    /// <typeparam name="TResponse">The type of the response object</typeparam>
    /// <param name="request">The request object containing parameters</param>
    /// <param name="requestSupplier">The supplier that creates the API call</param>
    /// <param name="responseType">The type of the response</param>
    /// <returns>The response object containing the API result</returns>
    public abstract Task<TResponse> ExecuteRequest<TData, TParam, TRequest, TResponse>(
        TRequest request,
        Func<TParam, Task<TData>> requestSupplier,
        Type responseType)
        where TRequest : class, IClientRequest<TParam>
        where TResponse : class, IClientResponse<TData>, new();

    /// <summary>
    /// Executes a streaming API request and returns a response with different response and stream types.
    /// </summary>
    /// <typeparam name="TData">Type of response body</typeparam>
    /// <typeparam name="TStream">Type of each element in the stream</typeparam>
    /// <typeparam name="TParam">Request parameter type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="request">The request to send</param>
    /// <param name="requestSupplier">Factory that creates the API call</param>
    /// <param name="responseType">The response class type</param>
    /// <param name="streamItemType">The class of the stream element</param>
    /// <returns>Streaming client response</returns>
    public abstract Task<TResponse> BiStreamRequest<TData, TStream, TParam, TRequest, TResponse>(
        TRequest request,
        Func<TParam, Task<Stream>> requestSupplier,
        Type responseType,
        Type streamItemType)
        where TRequest : class, IClientRequest<TParam>
        where TResponse : class, IStreamableClientResponse<TData, TStream>, new();

    /// <summary>
    /// Executes a streaming API request with the same type for response body and stream element.
    /// </summary>
    /// <typeparam name="T">Data type for both response and stream</typeparam>
    /// <typeparam name="TParam">Request parameter type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="request">The request to send</param>
    /// <param name="requestSupplier">Factory that creates the API call</param>
    /// <param name="responseType">The response class type</param>
    /// <returns>Streaming client response</returns>
    public virtual async Task<TResponse> StreamRequest<T, TParam, TRequest, TResponse>(
        TRequest request,
        Func<TParam, Task<Stream>> requestSupplier,
        Type responseType)
        where TRequest : class, IClientRequest<TParam>
        where TResponse : class, IStreamableClientResponse<T, T>, new()
    {
        return await BiStreamRequest<T, T, TParam, TRequest, TResponse>(
            request, requestSupplier, responseType, typeof(T));
    }

    /// <summary>
    /// Executes a synchronous API call and handles errors.
    /// </summary>
    /// <typeparam name="T">The type of the response</typeparam>
    /// <param name="apiCall">The API call to execute</param>
    /// <returns>The response from the API call</returns>
    /// <exception cref="ZaiHttpException">If an HTTP error occurs</exception>
    protected static async Task<T> Execute<T>(Func<Task<T>> apiCall)
    {
        try
        {
            var response = await apiCall();
            return response;
        }
        catch (HttpRequestException ex)
        {
            throw new ZaiHttpException(ex.Message, ex.StatusCode?.ToString(), ex);
        }
        catch (Exception ex) when (ex is TaskCanceledException && ex.InnerException is TimeoutException)
        {
            throw new ZaiHttpException("Request timed out", "Timeout", ex);
        }
        catch (Exception ex)
        {
            throw new ZaiHttpException(ex.Message, "UnknownError", ex);
        }
    }
}