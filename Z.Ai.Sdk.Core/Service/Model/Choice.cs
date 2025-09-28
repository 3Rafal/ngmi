using System.Collections.Generic;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Service.Agents;

namespace Z.Ai.Sdk.Core.Service.Model;

public record Choice(
    [property: JsonPropertyName("finish_reason")] string FinishReason,
    [property: JsonPropertyName("index")] long Index,
    [property: JsonPropertyName("message")] ChatMessage Message,
    [property: JsonPropertyName("messages")] List<AgentMessage> Messages,
    [property: JsonPropertyName("delta")] Delta Delta
);