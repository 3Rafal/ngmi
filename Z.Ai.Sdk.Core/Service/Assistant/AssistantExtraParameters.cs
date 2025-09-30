namespace Z.Ai.Sdk.Core.Service.Assistant;

/// <summary>
/// Extra parameters for assistant configuration. This class contains additional optional
/// parameters that can be used to customize assistant behavior and functionality.
/// </summary>
public record AssistantExtraParameters
{
    /// <summary>
    /// Translation agent parameters
    /// </summary>
    public AssistantTranslateParameters? Translate { get; init; }
}