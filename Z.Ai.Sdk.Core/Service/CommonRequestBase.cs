using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Z.Ai.Sdk.Core.Service;

/// <summary>
/// Base class for API requests that share common fields like request and user identifiers.
/// </summary>
public abstract record CommonRequestBase
{
    /// <summary>
    /// Request ID provided by the client, must be unique. If not provided the platform generates one.
    /// </summary>
    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }

    /// <summary>
    /// A unique identifier representing the end-user, used for abuse monitoring.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; init; }

    /// <summary>
    /// Arbitrary extra JSON fields forwarded to the API.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtraJson { get; init; }
}
