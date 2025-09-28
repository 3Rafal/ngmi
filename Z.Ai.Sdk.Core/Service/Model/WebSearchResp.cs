using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service.Model;

public record WebSearchResp(
    string Refer,
    string Title,
    string Link,
    string Media,
    string Content,
    string Icon,
    [property: JsonPropertyName("publish_date")] string PublishDate,
    List<string> Images
);