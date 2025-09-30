using System.IO;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Audio;

/// <summary>
/// Response wrapper for text-to-speech generation requests.
/// </summary>
public class AudioSpeechResponse : IClientResponse<FileInfo>
{
    public int Code { get; set; }

    public string? Msg { get; set; }

    public bool Success { get; set; }

    public FileInfo? Data { get; set; }

    public ChatError? Error { get; set; }
}
