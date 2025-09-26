# Plan: Rewriting z-ai-sdk-java to .NET

This document outlines a detailed plan for rewriting the `z-ai-sdk-java` project to a .NET equivalent. The target framework is .NET 9.

Before each step, check z-ai-sdk-java project directory and confirm what needs to be done.

## Phase 1: Project Setup and Core Infrastructure

This phase focuses on setting up the solution structure and porting the core components that are fundamental to the SDK's operation.

### [x] Step 1.1: Solution and Project Setup

1.  **Create a new .NET Solution:**
    *   Name: `Z.Ai.Sdk`
    *   Create a solution file `Z.Ai.Sdk.sln`.
2.  **Create Core Project:**
    *   Type: Class Library
    *   Name: `Z.Ai.Sdk.Core`
    *   Target Framework: `net9.0`
3.  **Create Samples Project:**
    *   Type: Console Application
    *   Name: `Z.Ai.Sdk.Samples`
    *   Target Framework: `net9.0`
    *   Add a project reference to `Z.Ai.Sdk.Core`.
4.  **Create Test Project:**
    *   Type: xUnit Test Project
    *   Name: `Z.Ai.Sdk.Core.Tests`
    *   Target Framework: `net9.0`
    *   Add a project reference to `Z.Ai.Sdk.Core`.

### [x] Step 1.2: Add NuGet Packages to `Z.Ai.Sdk.Core`

1.  **Refit:** For declarative, type-safe REST API calls.
    *   `Refit`
    *   `Refit.HttpClientFactory`
    *   `Refit.Newtonsoft.Json` (or `Refit.SystemTextJson` if we prefer)
2.  **System.Text.Json:** For JSON serialization and deserialization.
    *   `System.Text.Json`
3.  **System.IdentityModel.Tokens.Jwt:** For JWT creation.
    *   `System.IdentityModel.Tokens.Jwt`
4.  **Microsoft.Extensions.Http:** For `HttpClientFactory`.
    *   `Microsoft.Extensions.Http`
5.  **System.Reactive:** For reactive extensions (if we decide to port RxJava parts directly).
    *   `System.Reactive`

### [x] Step 1.3: Port Configuration (ZaiConfig)

1. Create a ZaiConfig.cs file in Z.Ai.Sdk.Core/Config.
2. Define ZaiConfig as a C# record with properties corresponding to the fields in the
    original Java ZaiConfig class.
3. Utilize primary constructors and optional parameters with default values to allow for
    flexible and clean object initialization, which replaces the need for a separate
    builder pattern.

### [x] Step 1.4: Port Constants

1.  Create a `Constants.cs` file in `Z.Ai.Sdk.Core`.
2.  Define a static `Constants` class.
3.  Port all constants from `Constants.java` to `Constants.cs`.

### [ ] Step 1.5: Port HTTP Client and Interceptor

1.  Create an `AuthenticationHeaderHandler.cs` that inherits from `DelegatingHandler`. This will be the equivalent of `HttpRequestInterceptor.java`.
2.  This handler will be responsible for adding the `Authorization` header to each request. It will use a `TokenManager` to get the token.
3.  Configure `HttpClientFactory` to create `HttpClient` instances with this handler.

### [ ] Step 1.6: Port Token Management

1.  Create an `ITokenManager.cs` interface.
2.  Create a `GlobalTokenManager.cs` implementation that handles JWT generation and caching.
3.  Use `System.IdentityModel.Tokens.Jwt` for JWT creation.
4.  Implement a simple in-memory cache for tokens, or use `IMemoryCache` from `Microsoft.Extensions.Caching.Memory`.

### [ ] Step 1.7: Port Core Models

1.  Create `ClientRequest.cs`, `ClientResponse.cs`, `StreamableClientResponse.cs` in `Z.Ai.Sdk.Core/Models`.
2.  These will be generic classes to standardize request and response handling. For streaming, `IAsyncEnumerable<T>` will be used instead of `Flowable`.

### [ ] Step 1.8: Port Abstract AI Client

1.  Create an `AbstractAiClient.cs` class.
2.  This class will hold the `HttpClient` and the Refit-generated API instances.
3.  It will contain the logic for executing requests (both synchronous and streaming).
4.  It will have properties for accessing the different services (e.g., `ChatService`, `EmbeddingService`), which will be lazily initialized.

## Phase 2: Porting Services (Vertical Slices)

This phase involves porting each AI service one by one. Each step is a self-contained vertical slice that includes the API definition, models, service, tests, and a sample.

### [ ] Step 2.1: Port Chat Service

1.  **Port `ChatApi.java` to `IChatApi.cs`:**
    *   Create the interface in `Z.Ai.Sdk.Core/Apis`.
    *   Use Refit attributes (`[Get]`, `[Post]`, `[Body]`, etc.) to define the endpoints.
    *   For streaming, the method will return `Task<HttpContent>`.
2.  **Port Chat Models:**
    *   Port `ChatCompletionCreateParams.java`, `ChatMessage.java`, `ModelData.java`, etc., to C# records or classes in `Z.Ai.Sdk.Core/Models/Chat`.
    *   Use `System.Text.Json.Serialization` attributes (`[JsonPropertyName]`) for JSON mapping.
3.  **Port `ChatService.java` to `IChatService.cs` and `ChatService.cs`:**
    *   Create the interface in `Z.Ai.Sdk.Core/Services`.
    *   Create the implementation in `Z.Ai.Sdk.Core/Services`.
    *   The implementation will use the `IChatApi` to make the API calls.
4.  **Write Unit Tests for `ChatService`:**
    *   In `Z.Ai.Sdk.Core.Tests`, create `ChatServiceTests.cs`.
    *   Use a mocking library like `Moq` to mock the `IChatApi` and `HttpClient`.
    *   Write tests for synchronous, asynchronous, and streaming chat completions.
5.  **Port `ChatCompletionExample.java` and `ChatCompletionStreamExample.java`:**
    *   In `Z.Ai.Sdk.Samples`, create `ChatCompletionExample.cs` and `ChatCompletionStreamExample.cs`.
    *   These examples will demonstrate how to use the new .NET SDK to perform chat completions.

### [ ] Step 2.2: Port Embedding Service

1.  **Port `EmbeddingApi.java` to `IEmbeddingApi.cs`**.
2.  **Port Embedding Models:** `EmbeddingCreateParams.java`, `EmbeddingResponse.java`, etc.
3.  **Port `EmbeddingService.java` to `IEmbeddingService.cs` and `EmbeddingService.cs`**.
4.  **Write Unit Tests for `EmbeddingService`**.
5.  **Port `EmbeddingsExample.java`**.

### [ ] Step 2.3: Port Image Service

1.  **Port `ImagesApi.java` to `IImagesApi.cs`**.
2.  **Port Image Models:** `CreateImageRequest.java`, `ImageResponse.java`, etc.
3.  **Port `ImageService.java` to `IImageService.cs` and `ImageService.cs`**.
4.  **Write Unit Tests for `ImageService`**.
5.  **Port `ImageGenerationExample.java`**.

**(Repeat for all other services: `Agents`, `Assistant`, `Audio`, `Batches`, `File`, `Tools`, `Videos`, `VoiceClone`, `WebSearch`)**

## Phase 3: Finalization and Documentation

This phase focuses on the final touches to make the project complete and easy to use.

### [ ] Step 3.1: Port Remaining Samples

1.  Port all remaining examples from the Java `samples` directory to the .NET `Z.Ai.Sdk.Samples` project.
2.  Ensure all samples are runnable and demonstrate the SDK's features correctly.

### [ ] Step 3.2: Create Documentation

1.  **Create `README.md`:**
    *   Write a new `README.md` for the .NET project.
    *   Include installation instructions (NuGet), a quick start guide, and basic usage examples for the most common services.
2.  **Create `CONTRIBUTING.md`:**
    *   Adapt the existing `CONTRIBUTING.md` for the .NET project.
3.  **Add XML Documentation Comments:**
    *   Add detailed XML documentation comments to all public classes, methods, and properties in `Z.Ai.Sdk.Core`. This will enable IntelliSense in Visual Studio and other IDEs.

### [ ] Step 3.3: Code Review and Refinement

1.  Perform a full code review of the entire `Z.Ai.Sdk` solution.
2.  Ensure consistency in coding style, naming conventions, and error handling.
3.  Refine the API to make it more idiomatic for .NET developers.

### [ ] Step 3.4: CI/CD Setup

1.  Create a GitHub Actions workflow for the .NET project.
2.  The workflow should:
    *   Restore dependencies.
    *   Build the solution.
    *   Run all tests.
    *   Pack the `Z.Ai.Sdk.Core` project into a NuGet package.
    *   (Optional) Publish the NuGet package to a feed.
