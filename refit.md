# .NET Refit Implementation Plan

## Overview
This document outlines the complete implementation plan for integrating Refit into the .NET Z.AI SDK, following the same architectural patterns as the Java Retrofit implementation.

## Current Status: Phase 1 ✅ Complete

### Completed Infrastructure
- ✅ **RefitServiceFactory.cs** - Factory for creating Refit services with authentication
- ✅ **AbstractAiClient.cs** - Updated with `CreateRefitService<T>()` and `CreateRefitStreamingService<T>()` methods
- ✅ **Build Integration** - All infrastructure compiles successfully

---

## Phase 2: API Interface Definitions

### 2.1 Directory Structure
```
Z.Ai.Sdk.Core/
├── Api/                           # New directory for Refit interfaces
│   ├── Chat/
│   │   └── IChatApi.cs
│   ├── Embedding/
│   │   └── IEmbeddingApi.cs
│   ├── Agents/
│   │   └── IAgentsApi.cs
│   ├── File/
│   │   └── IFileApi.cs
│   ├── Audio/
│   │   └── IAudioApi.cs
│   ├── Images/
│   │   └── IImagesApi.cs
│   ├── Batches/
│   │   └── IBatchesApi.cs
│   ├── Videos/
│   │   └── IVideosApi.cs
│   ├── Assistant/
│   │   └── IAssistantApi.cs
│   ├── VoiceClone/
│   │   └── IVoiceCloneApi.cs
│   └── WebSearch/
│       └── IWebSearchApi.cs
```

### 2.2 API Interface Templates

#### IChatApi.cs
```csharp
using Refit;
using Z.Ai.Sdk.Core.Models.Chat;

namespace Z.Ai.Sdk.Core.Api.Chat;

public interface IChatApi
{
    [Post("/chat/completions")]
    Task<ChatCompletionResponse> CreateChatCompletionAsync([Body] ChatCompletionRequest request);

    [Post("/chat/completions")]
    Task<ApiResponse<Stream>> CreateChatCompletionStreamAsync([Body] ChatCompletionRequest request);

    [Post("/chat/completions")]
    Task<ChatCompletionResponse> CreateChatCompletionWithHeadersAsync(
        [Body] ChatCompletionRequest request,
        [HeaderCollection] Dictionary<string, string> headers);

    [Get("/models")]
    Task<ModelsResponse> ListModelsAsync();

    [Get("/models/{modelId}")]
    Task<ModelResponse> GetModelAsync([AliasAs("modelId")] string modelId);
}
```

#### IEmbeddingApi.cs
```csharp
using Refit;
using Z.Ai.Sdk.Core.Models.Embedding;

namespace Z.Ai.Sdk.Core.Api.Embedding;

public interface IEmbeddingApi
{
    [Post("/embeddings")]
    Task<EmbeddingResponse> CreateEmbeddingAsync([Body] EmbeddingRequest request);

    [Post("/embeddings")]
    Task<EmbeddingResponse> CreateEmbeddingWithHeadersAsync(
        [Body] EmbeddingRequest request,
        [HeaderCollection] Dictionary<string, string> headers);
}
```

#### IFileApi.cs
```csharp
using Refit;
using Z.Ai.Sdk.Core.Models.File;

namespace Z.Ai.Sdk.Core.Api.File;

public interface IFileApi
{
    [Get("/files")]
    Task<FileListResponse> ListFilesAsync();

    [Post("/files")]
    Task<FileUploadResponse> UploadFileAsync([AliasAs("file")] StreamPart stream, [Query] string purpose);

    [Get("/files/{fileId}")]
    Task<FileResponse> GetFileAsync([AliasAs("fileId")] string fileId);

    [Delete("/files/{fileId}")]
    Task<DeleteResponse> DeleteFileAsync([AliasAs("fileId")] string fileId);

    [Get("/files/{fileId}/content")]
    Task<ApiResponse<Stream>> DownloadFileAsync([AliasAs("fileId")] string fileId);
}
```

### 2.3 Model Classes to Create
Each API interface will require corresponding request/response model classes:

#### Chat Models
- `ChatCompletionRequest` - Request parameters for chat completion
- `ChatCompletionResponse` - Response from chat completion
- `ChatMessage` - Individual chat message
- `ModelsResponse` - List of available models
- `ModelResponse` - Individual model information

#### Embedding Models
- `EmbeddingRequest` - Text embedding request
- `EmbeddingResponse` - Embedding response with vectors
- `EmbeddingData` - Individual embedding data

#### File Models
- `FileUploadRequest` - File upload parameters
- `FileUploadResponse` - Upload response
- `FileListResponse` - List of uploaded files
- `FileResponse` - Individual file information

---

## Phase 3: Service Implementation Updates

### 3.1 Service Class Updates
Update all service classes to use Refit API clients:

#### ChatService.cs Update
```csharp
using Z.Ai.Sdk.Core.Api.Chat;

namespace Z.Ai.Sdk.Core.Services.Chat;

public class ChatService : IChatService
{
    private readonly IChatApi _chatApi;

    public ChatService(AbstractAiClient client)
    {
        _chatApi = client.CreateRefitService<IChatApi>();
    }

    public async Task<ChatCompletionResponse> CreateChatCompletionAsync(ChatCompletionRequest request)
    {
        return await _chatApi.CreateChatCompletionAsync(request);
    }

    public async IAsyncEnumerable<ChatCompletionChunk> StreamChatCompletionAsync(ChatCompletionRequest request)
    {
        var response = await _chatApi.CreateChatCompletionStreamAsync(request);

        await foreach (var chunk in ParseSseStream(response.Content))
        {
            yield return chunk;
        }
    }
}
```

### 3.2 Service Implementation Pattern
All services will follow this pattern:
1. **Constructor** - Get Refit API client from `AbstractAiClient.CreateRefitService<T>()`
2. **Methods** - Call Refit API methods directly
3. **Error Handling** - Leverage existing AbstractAiClient error handling
4. **Streaming** - Use `CreateRefitStreamingService<T>()` for SSE operations

---

## Phase 4: Advanced Features Implementation

### 4.1 Streaming Support (SSE)
Implement Server-Sent Events streaming:

```csharp
private static async IAsyncEnumerable<T> ParseSseStream<T>(Stream stream)
{
    using var reader = new StreamReader(stream);
    while (!reader.EndOfStream)
    {
        var line = await reader.ReadLineAsync();
        if (line?.StartsWith("data: ") == true)
        {
            var data = line["data: ".Length..];
            if (data != "[DONE]")
            {
                var item = JsonSerializer.Deserialize<T>(data, _jsonOptions);
                if (item != null)
                {
                    yield return item;
                }
            }
        }
    }
}
```

### 4.2 Reactive Extensions Support
Add support for `IObservable<T>` and reactive patterns:

```csharp
public IObservable<ChatCompletionChunk> StreamChatCompletionObservable(ChatCompletionRequest request)
{
    return Observable.FromAsync(async token =>
    {
        var response = await _chatApi.CreateChatCompletionStreamAsync(request);
        return ParseSseStream(response.Content).ToObservable();
    })
    .Switch();
}
```

### 4.3 Custom Headers and Authentication
Extend existing authentication for Refit:

```csharp
[Post("/chat/completions")]
Task<ChatCompletionResponse> CreateChatCompletionWithCustomHeadersAsync(
    [Body] ChatCompletionRequest request,
    [Header("X-Custom-Header")] string customHeader,
    [Header("Authorization")] string authorization);
```

---

## Phase 5: Testing and Validation

### 5.1 Unit Tests Structure
```
Z.Ai.Sdk.Core.Tests/
├── Api/
│   ├── ChatApiTests.cs
│   ├── EmbeddingApiTests.cs
│   └── ...
├── Services/
│   ├── ChatServiceTests.cs
│   ├── EmbeddingServiceTests.cs
│   └── ...
└── Integration/
    ├── RefitIntegrationTests.cs
    └── StreamingTests.cs
```

### 5.2 Test Examples
```csharp
[Test]
public async Task CreateChatCompletion_ValidRequest_ReturnsResponse()
{
    // Arrange
    var api = new Mock<IChatApi>();
    var service = new ChatService(api.Object);

    // Act
    var response = await service.CreateChatCompletionAsync(testRequest);

    // Assert
    response.Should().NotBeNull();
    response.Choices.Should().NotBeEmpty();
}
```

### 5.3 Integration Tests
- End-to-end API testing with mock servers
- Streaming functionality validation
- Error handling and edge cases
- Performance benchmarking vs. current implementation

---

## Implementation Timeline

### Phase 2: API Interfaces (2-3 days)
- **Day 1:** Chat and Embedding APIs with models
- **Day 2:** File, Audio, and Image APIs with models
- **Day 3:** Remaining APIs (Batches, Videos, Assistant, VoiceClone, WebSearch)

### Phase 3: Service Updates (1-2 days)
- Update all 11 service classes to use Refit
- Maintain existing public interfaces
- Ensure backward compatibility

### Phase 4: Advanced Features (1-2 days)
- Streaming implementation
- Reactive extensions support
- Custom headers and advanced scenarios

### Phase 5: Testing (1-2 days)
- Unit tests for all APIs and services
- Integration tests
- Performance validation

**Total Estimated Time:** 5-9 days

---

## Key Benefits

### 1. **Consistency with Java SDK**
- Same architectural patterns and naming conventions
- Identical API interface structure
- Consistent error handling approaches

### 2. **Type Safety**
- Compile-time validation of API endpoints
- Automatic serialization/deserialization
- Strong typing for request/response models

### 3. **Reduced Boilerplate**
- Automatic HTTP client generation
- No manual HTTP request building
- Built-in error handling

### 4. **Performance**
- Efficient HTTP client usage
- Connection pooling and reuse
- Optimized serialization

### 5. **Maintainability**
- Clear separation between API definition and implementation
- Easy to modify and extend
- Consistent patterns across all services

---

## Migration Strategy

### Step 1: Parallel Implementation
- Keep existing implementation running
- Implement Refit version alongside
- Gradual migration of services

### Step 2: Feature Parity
- Ensure all existing features work with Refit
- Performance benchmarking
- Memory usage validation

### Step 3: Switch Over
- Replace existing implementation with Refit version
- Update documentation
- Deprecate old HTTP client methods

### Step 4: Optimization
- Performance tuning
- Memory optimization
- Additional features (caching, retries, etc.)

---

## Risk Mitigation

### 1. **Breaking Changes**
- Maintain existing public interfaces
- Use adapter pattern if needed
- Provide migration guide

### 2. **Performance Issues**
- Benchmark against current implementation
- Optimize JSON serialization
- Monitor memory usage

### 3. **Compatibility**
- Test with all .NET target versions
- Validate Refit version compatibility
- Ensure third-party integrations work

### 4. **Testing Coverage**
- Comprehensive unit tests
- Integration tests for all APIs
- Load testing for performance validation

---

## Success Criteria

### 1. **Functional Requirements**
- ✅ All existing API endpoints work with Refit
- ✅ Streaming functionality maintained
- ✅ Authentication and authorization working
- ✅ Error handling consistent

### 2. **Non-Functional Requirements**
- ✅ Performance equal or better than current implementation
- ✅ Memory usage optimized
- ✅ Build and test automation working
- ✅ Documentation updated

### 3. **Quality Requirements**
- ✅ Code coverage > 80%
- ✅ All tests passing
- ✅ No breaking changes to public API
- ✅ Consistent coding standards

---

## Next Steps

1. **Immediate:** Start Phase 2 - Create API interfaces for Chat and Embedding services
2. **Short-term:** Complete all API interfaces and basic service implementations
3. **Medium-term:** Implement advanced features and comprehensive testing
4. **Long-term:** Performance optimization and production deployment

This implementation plan provides a clear roadmap for successfully integrating Refit into the .NET Z.AI SDK while maintaining consistency with the Java implementation and ensuring a smooth transition.