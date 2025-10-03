using Z.Ai.Sdk.Core.Api.Videos;
using Z.Ai.Sdk.Core.Service.Model;

namespace Z.Ai.Sdk.Core.Service.Videos;

/// <summary>
/// Implementation of VideosService
/// </summary>
public class VideosService : IVideosService
{
    private readonly AbstractAiClient _client;
    private readonly IVideosApi _videosApi;

    /// <summary>
    /// Initializes a new instance of VideosService
    /// </summary>
    /// <param name="client">The AI client instance</param>
    public VideosService(AbstractAiClient client)
    {
        _client = client;
        _videosApi = client.CreateRefitService<IVideosApi>();
    }

    /// <summary>
    /// Creates video generations.
    /// </summary>
    /// <param name="request">the video generation request</param>
    /// <returns>VideosResponse containing the generation result</returns>
    public async Task<VideosResponse> VideoGenerationsAsync(VideoCreateParams request)
    {
        ValidateParams(request);
        return await _client.ExecuteRequest<VideoObject, VideoCreateParams, VideoCreateParams, VideosResponse>(
            request,
            _videosApi.VideoGenerations,
            typeof(VideosResponse));
    }

    /// <summary>
    /// Retrieves video generation result.
    /// </summary>
    /// <param name="taskId">the task ID to retrieve</param>
    /// <returns>VideosResponse containing the generation result</returns>
    public async Task<VideosResponse> VideoGenerationsResultAsync(string taskId)
    {
        ValidateTaskId(taskId);
        var retrieveRequest = new AsyncResultRetrieveParams(taskId);
        return await _client.ExecuteRequest<VideoObject, AsyncResultRetrieveParams, AsyncResultRetrieveParams, VideosResponse>(
            retrieveRequest,
            parameters => _videosApi.VideoGenerationsResult(parameters.TaskId),
            typeof(VideosResponse));
    }

    /// <summary>
    /// Validates the video creation parameters
    /// </summary>
    /// <param name="request">The request to validate</param>
    /// <exception cref="ArgumentException">If validation fails</exception>
    private void ValidateParams(VideoCreateParams request)
    {
        if (request == null)
        {
            throw new ArgumentException("request cannot be null", nameof(request));
        }
        if (string.IsNullOrEmpty(request.Model))
        {
            throw new ArgumentException("request model cannot be null or empty", nameof(request.Model));
        }
        if (string.IsNullOrEmpty(request.Prompt))
        {
            throw new ArgumentException("request prompt cannot be null or empty", nameof(request.Prompt));
        }
    }

    /// <summary>
    /// Validates the task ID parameter
    /// </summary>
    /// <param name="taskId">The task ID to validate</param>
    /// <exception cref="ArgumentException">If validation fails</exception>
    private void ValidateTaskId(string taskId)
    {
        if (string.IsNullOrEmpty(taskId))
        {
            throw new ArgumentException("taskId cannot be null or empty", nameof(taskId));
        }
    }
}