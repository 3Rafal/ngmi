using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Assistant.QuerySupport;

/// <summary>
/// Parameters for querying assistant support status. This class contains the parameters
/// needed to query the support status of specific assistants.
/// </summary>
public record AssistantQuerySupportParams : IClientRequest<AssistantQuerySupportParams>
{
    /// <summary>
    /// List of assistant IDs to query support status for.
    /// </summary>
    [JsonPropertyName("assistant_id_list")]
    public List<string>? AssistantIdList { get; init; }
}