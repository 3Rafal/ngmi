using System.Collections.Generic;

namespace Z.Ai.Sdk.Core.Service.Model;

public record InputSchema(
    string Type,
    Dictionary<string, object> Properties,
    List<string> Required,
    bool? AdditionalProperties
);