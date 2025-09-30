using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message;

/// <summary>
/// Base class for assistant message content types.
/// </summary>
[JsonConverter(typeof(AssistantMessageContentJsonConverter))]
public abstract record AssistantMessageContent
{
}