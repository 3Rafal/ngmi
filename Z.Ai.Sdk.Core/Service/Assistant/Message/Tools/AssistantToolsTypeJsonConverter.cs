using System.Text.Json;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Assistant.Message.Tools;

internal sealed class AssistantToolsTypeJsonConverter : JsonConverter<AssistantToolsType>
{
    public override AssistantToolsType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("Assistant tool call must be a JSON object.");
        }

        var resolvedType = ResolveToolType(root);

        return resolvedType switch
        {
            "web_browser" => new AssistantWebBrowserToolBlock
            {
                Type = resolvedType,
                Url = TryGetString(root, "url")
            },
            "retrieval" => new AssistantRetrievalToolBlock
            {
                Type = resolvedType
            },
            "function" => ReadFunctionTool(root, options, resolvedType),
            "code_interpreter" => new AssistantCodeInterpreterToolBlock
            {
                Type = resolvedType,
                Code = TryGetString(root, "code"),
                Input = TryGetString(root, "input"),
                Output = TryGetString(root, "output")
            },
            "drawing_tool" => new AssistantDrawingToolBlock
            {
                Type = resolvedType,
                Prompt = TryGetString(root, "prompt")
            },
            _ => new AssistantUnknownToolBlock
            {
                Type = resolvedType,
                Raw = SafeGetRawText(root)
            }
        };
    }

    public override void Write(Utf8JsonWriter writer, AssistantToolsType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AssistantWebBrowserToolBlock webBrowser:
                JsonSerializer.Serialize(writer, new Dictionary<string, object?>
                {
                    ["type"] = webBrowser.Type ?? "web_browser",
                    ["url"] = webBrowser.Url
                }, options);
                break;
            case AssistantRetrievalToolBlock retrieval:
                JsonSerializer.Serialize(writer, new Dictionary<string, object?>
                {
                    ["type"] = retrieval.Type ?? "retrieval"
                }, options);
                break;
            case AssistantFunctionToolBlock functionBlock:
                JsonSerializer.Serialize(writer, new Dictionary<string, object?>
                {
                    ["type"] = functionBlock.Type ?? "function",
                    ["function"] = functionBlock.Function
                }, options);
                break;
            case AssistantCodeInterpreterToolBlock codeInterpreter:
                JsonSerializer.Serialize(writer, new Dictionary<string, object?>
                {
                    ["type"] = codeInterpreter.Type ?? "code_interpreter",
                    ["code"] = codeInterpreter.Code,
                    ["input"] = codeInterpreter.Input,
                    ["output"] = codeInterpreter.Output
                }, options);
                break;
            case AssistantDrawingToolBlock drawing:
                JsonSerializer.Serialize(writer, new Dictionary<string, object?>
                {
                    ["type"] = drawing.Type ?? "drawing_tool",
                    ["prompt"] = drawing.Prompt
                }, options);
                break;
            case AssistantUnknownToolBlock unknown when !string.IsNullOrWhiteSpace(unknown.Raw):
                using (var document = JsonDocument.Parse(unknown.Raw))
                {
                    document.RootElement.WriteTo(writer);
                }
                break;
            case AssistantUnknownToolBlock:
                writer.WriteStartObject();
                writer.WriteEndObject();
                break;
            default:
                throw new JsonException($"Unsupported assistant tool type: {value.GetType()}");
        }
    }

    private static AssistantToolsType ReadFunctionTool(JsonElement root, JsonSerializerOptions options, string? resolvedType)
    {
        AssistantFunctionToolBlock.AssistantFunction? function = null;
        if (root.TryGetProperty("function", out var functionElement) && functionElement.ValueKind == JsonValueKind.Object)
        {
            function = JsonSerializer.Deserialize<AssistantFunctionToolBlock.AssistantFunction>(functionElement.GetRawText(), options);
        }

        return new AssistantFunctionToolBlock
        {
            Type = resolvedType ?? "function",
            Function = function
        };
    }

    private static string? ResolveToolType(JsonElement root)
    {
        if (root.TryGetProperty("type", out var typeElement) && typeElement.ValueKind == JsonValueKind.String)
        {
            return typeElement.GetString();
        }

        if (root.TryGetProperty("function", out _))
        {
            return "function";
        }

        if (root.TryGetProperty("url", out _))
        {
            return "web_browser";
        }

        if (root.TryGetProperty("code", out _) || root.TryGetProperty("input", out _) || root.TryGetProperty("output", out _))
        {
            return "code_interpreter";
        }

        if (root.TryGetProperty("prompt", out _))
        {
            return "drawing_tool";
        }

        return null;
    }

    private static string? TryGetString(JsonElement root, string propertyName)
    {
        return root.TryGetProperty(propertyName, out var element) && element.ValueKind == JsonValueKind.String
            ? element.GetString()
            : null;
    }

    private static string? SafeGetRawText(JsonElement element)
    {
        try
        {
            return element.GetRawText();
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}
