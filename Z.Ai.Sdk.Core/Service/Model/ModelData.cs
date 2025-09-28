using System.Collections.Generic;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Service.Agents;

namespace Z.Ai.Sdk.Core.Service.Model;

public record ModelData(
    [property: JsonPropertyName("choices")] List<Choice> Choices,
    [property: JsonPropertyName("usage")] Usage Usage,
    [property: JsonPropertyName("request_id")] string RequestId,
    [property: JsonPropertyName("task_status")] TaskStatus TaskStatus,
    long Created,
    string Model,
    string Id,
    [property: JsonPropertyName("agent_id")] string AgentId,
    [property: JsonPropertyName("conversation_id")] string ConversationId,
    [property: JsonPropertyName("async_id")] string AsyncId,
    string Status,
    [property: JsonPropertyName("web_search")] List<WebSearchResp> WebSearch,
    string Type,
    string Text,
    List<Segment> Segments,
    string Delta
);