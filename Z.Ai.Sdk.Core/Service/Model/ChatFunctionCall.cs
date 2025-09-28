using System.Text.Json;

namespace Z.Ai.Sdk.Core.Service.Model;

public record ChatFunctionCall(
    string Name,
    JsonElement Arguments
);