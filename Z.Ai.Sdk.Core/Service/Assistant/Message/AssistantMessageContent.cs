using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message;

/// <summary>
/// Base class for assistant message content types.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(AssistantTextContentBlock), "content")]
[JsonDerivedType(typeof(AssistantToolsDeltaBlock), "tool_calls")]
public abstract record AssistantMessageContent
{
}