using System.Collections.Generic;
using System.Text.Json.Serialization;
using Z.Ai.Sdk.Core.Model;

namespace Z.Ai.Sdk.Core.Service.Model;

/// <summary>
/// Create file upload request
/// </summary>
public record FileUploadParams(
    /// <summary>
    /// The purpose of the file
    /// </summary>
    string? Purpose = null,

    /// <summary>
    /// Local file path
    /// </summary>
    string? FilePath = null
) : CommonRequestBase, IClientRequest<FileUploadParams>;