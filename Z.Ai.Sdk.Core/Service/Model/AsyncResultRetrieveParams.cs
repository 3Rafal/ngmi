using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Parameters for retrieving async result
/// </summary>
public record AsyncResultRetrieveParams : IClientRequest<AsyncResultRetrieveParams>
{
    /// <summary>
    /// Task ID
    /// </summary>
    [JsonPropertyName("task_id")]
    public string TaskId { get; init; }

    public AsyncResultRetrieveParams(string taskId)
    {
        TaskId = taskId;
    }
}