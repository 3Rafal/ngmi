using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message;

internal sealed class AssistantMessageContentJsonConverter : JsonConverter<AssistantMessageContent>
{
    public override AssistantMessageContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("Assistant message content must be a JSON object.");
        }

        if (TryReadToolCalls(root, options, out var toolsBlock))
        {
            return toolsBlock;
        }

        return ReadTextBlock(root);
    }

    public override void Write(Utf8JsonWriter writer, AssistantMessageContent value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AssistantToolsDeltaBlock toolsDelta:
                JsonSerializer.Serialize(writer, toolsDelta, options);
                break;
            case AssistantTextContentBlock textDelta:
                JsonSerializer.Serialize(writer, textDelta, options);
                break;
            default:
                throw new JsonException($"Unsupported assistant message content type: {value.GetType()}");
        }
    }

    private static bool TryReadToolCalls(JsonElement root, JsonSerializerOptions options, out AssistantToolsDeltaBlock? result)
    {
        result = null;
        if (!root.TryGetProperty("tool_calls", out var toolCallsElement))
        {
            return false;
        }

        List<AssistantToolsType>? toolCalls = toolCallsElement.ValueKind switch
        {
            JsonValueKind.Array => JsonSerializer.Deserialize<List<AssistantToolsType>>(toolCallsElement.GetRawText(), options),
            JsonValueKind.Object =>
                JsonSerializer.Deserialize<AssistantToolsType>(toolCallsElement.GetRawText(), options) is { } singleTool
                    ? new List<AssistantToolsType> { singleTool }
                    : new List<AssistantToolsType>(),
            _ => null
        };

        if (toolCalls is { Count: 0 })
        {
            toolCalls = null;
        }

        var role = root.TryGetProperty("role", out var roleElement) && roleElement.ValueKind == JsonValueKind.String
            ? roleElement.GetString()
            : null;

        result = new AssistantToolsDeltaBlock
        {
            ToolCalls = toolCalls,
            Role = role ?? "tool"
        };

        return true;
    }

    private static AssistantTextContentBlock ReadTextBlock(JsonElement root)
    {
        string? content = null;
        if (root.TryGetProperty("content", out var contentElement))
        {
            content = contentElement.ValueKind switch
            {
                JsonValueKind.String => contentElement.GetString(),
                JsonValueKind.Null => null,
                JsonValueKind.Array or JsonValueKind.Object => contentElement.GetRawText(),
                _ => contentElement.ToString()
            };
        }

        var role = root.TryGetProperty("role", out var roleElement) && roleElement.ValueKind == JsonValueKind.String
            ? roleElement.GetString()
            : null;

        return new AssistantTextContentBlock
        {
            Content = content,
            Role = role ?? "assistant"
        };
    }
}
