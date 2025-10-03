namespace Z.Ai.Sdk.Core.Service.Videos;

/// <summary>
/// Videos service interface
/// </summary>
public interface IVideosService
{
    /// <summary>
    /// Creates video generations.
    /// </summary>
    /// <param name="request">the video generation request</param>
    /// <returns>VideosResponse containing the generation result</returns>
    Task<VideosResponse> VideoGenerationsAsync(VideoCreateParams request);

    /// <summary>
    /// Retrieves video generation result.
    /// </summary>
    /// <param name="taskId">the task ID to retrieve</param>
    /// <returns>VideosResponse containing the generation result</returns>
    Task<VideosResponse> VideoGenerationsResultAsync(string taskId);
}