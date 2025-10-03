using Refit;
using Z.Ai.Sdk.Core.Service.Videos;

namespace Z.Ai.Sdk.Core.Api.Videos;

/// <summary>
/// Videos API for AI-powered video generation Powered by CogVideoX models using advanced
/// Transformer and 3D Causal VAE architecture Supports both text-to-video and
/// image-to-video generation with exceptional quality Features natural camera movements,
/// semantic coherence, and photorealistic visual output Configurable parameters include
/// quality, audio generation, size, and frame rate (fps)
/// </summary>
public interface IVideosApi
{
    /// <summary>
    /// Generate videos from text or image prompts using CogVideoX Creates high-quality
    /// videos with natural camera movements and semantic coherence Supports text-to-video
    /// generation with detailed prompt descriptions Supports image-to-video generation
    /// using image_url parameter for enhanced control
    /// </summary>
    /// <param name="request">Video generation parameters including prompt, image_url, quality,
    /// with_audio, size, and fps</param>
    /// <returns>Video generation task information with processing status and result URLs</returns>
    [Post("/videos/generations")]
    Task<VideoObject> VideoGenerations([Body] VideoCreateParams request);

    /// <summary>
    /// Retrieve the result of an asynchronous video generation Gets the generated video
    /// result using the task ID from video generation request Video generation is
    /// typically asynchronous due to computational complexity
    /// </summary>
    /// <param name="id">Task ID returned from video generation request</param>
    /// <returns>Generated video URLs with metadata including duration, resolution, and
    /// audio information</returns>
    [Get("/async-result/{id}")]
    Task<VideoObject> VideoGenerationsResult(string id);
}