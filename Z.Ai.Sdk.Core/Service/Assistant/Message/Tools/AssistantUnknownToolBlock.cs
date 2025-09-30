namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

/// <summary>
/// Represents an unknown or partially formed tool call payload.
/// </summary>
public record AssistantUnknownToolBlock : AssistantToolsType
{
    /// <summary>
    /// Raw JSON representation of the tool payload when it cannot be mapped to a known type.
    /// </summary>
    public string? Raw { get; init; }
}
