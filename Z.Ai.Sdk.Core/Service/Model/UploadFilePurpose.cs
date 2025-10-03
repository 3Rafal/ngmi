namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Enum for file upload purposes
/// Preserves 100% of Java logic from UploadFilePurpose
/// </summary>
public enum UploadFilePurpose
{
    /// <summary>
    /// Batch processing purpose
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("batch")]
    Batch,

    /// <summary>
    /// File extraction purpose
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("file-extract")]
    FileExtract,

    /// <summary>
    /// Code interpreter purpose
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("code-interpreter")]
    CodeInterpreter,

    /// <summary>
    /// Agent purpose
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("agent")]
    Agent,

    /// <summary>
    /// Voice clone input purpose
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("voice-clone-input")]
    VoiceCloneInput
}

/// <summary>
/// Extension methods for UploadFilePurpose enum
/// </summary>
public static class UploadFilePurposeExtensions
{
    /// <summary>
    /// Gets the string value of the enum
    /// </summary>
    /// <param name="purpose">The enum value</param>
    /// <returns>The string representation</returns>
    public static string Value(this UploadFilePurpose purpose)
    {
        return purpose switch
        {
            UploadFilePurpose.Batch => "batch",
            UploadFilePurpose.FileExtract => "file-extract",
            UploadFilePurpose.CodeInterpreter => "code-interpreter",
            UploadFilePurpose.Agent => "agent",
            UploadFilePurpose.VoiceCloneInput => "voice-clone-input",
            _ => "unknown"
        };
    }
}