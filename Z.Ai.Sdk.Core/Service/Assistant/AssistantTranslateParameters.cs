namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// Parameters for translation operations. This class contains the source and target
/// language settings for translation requests.
/// </summary>
public record AssistantTranslateParameters
{
    /// <summary>
    /// Source language for translation.
    /// </summary>
    public string? From { get; init; }

    /// <summary>
    /// Target language for translation.
    /// </summary>
    public string? To { get; init; }
}