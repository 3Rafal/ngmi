using System.Collections.Immutable;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model
{
    public class ChatCompletionResponse : ISingleStreamableClientResponse<ModelData>
    {
        public int Code { get; set; }
        public string? Msg { get; set; }
        public bool Success { get; set; }
        public ModelData? Data { get; set; }
        public IAsyncEnumerable<ModelData> Stream { get; set; } = ImmutableArray<ModelData>.Empty.ToAsyncEnumerable();
        public ChatError? Error { get; set; }
    }
}
