# Z.ai Open Platform Java SDK: Project Documentation

## 1. Project Overview

The Z.ai Open Platform Java SDK is the official Java library for interacting with the Z.ai and Zhipu AI platforms. It provides a unified, type-safe, and high-performance interface for accessing a wide range of AI capabilities, including:

-   **Chat Completion:** Advanced conversational AI with models like GLM-4.
-   **Embeddings:** Text embedding generation for semantic search and other NLP tasks.
-   **Image Generation:** Creating images from text prompts using models like CogView.
-   **Audio Services:** Speech-to-text and text-to-speech functionalities.
-   **And more:** File management, AI assistants, batch processing, video analysis, and web search.

The SDK is designed for easy integration into Java 8+ projects, is built with modern libraries like OkHttp3, Retrofit2, and RxJava3, and provides a fluent builder pattern for client configuration.

## 2. High-Level Project Structure

The project is a multi-module Maven project, organized into a logical and maintainable structure.

```
z-ai-sdk-java/
├── .github/              # CI/CD workflows and issue/PR templates
├── core/                 # The core SDK module with all business logic
├── samples/              # Example usage code for different AI services
├── script/               # Utility scripts
├── .gitignore            # Git ignore file
├── .springjavaformatconfig # Configuration for the Spring Java code formatter
├── ARCHITECTURE.md       # High-level architecture overview
├── CONTRIBUTING.md       # Guidelines for contributing to the project
├── LICENSE               # MIT License file
├── pom.xml               # Parent Maven project configuration
└── README.md             # Main project README
```

---

## 3. Detailed File and Directory Descriptions

This section provides a granular breakdown of every file and directory within the project.

### 3.1. Root Directory (`z-ai-sdk-java/`)

-   `pom.xml`: The parent Maven Project Object Model. It defines the project modules (`core`, `samples`), manages shared properties and versions, and configures global build plugins for code formatting (`spring-javaformat-maven-plugin`), testing (`maven-surefire-plugin`), and deployment.
-   `README.md`: The primary documentation in English, offering a quick start guide, installation instructions, and basic usage examples.
-   `README_CN.md`: The Chinese version of the README file.
-   `Release-Note.md`: Contains detailed notes for each version release, documenting new features, bug fixes, and breaking changes.
-   `LICENSE`: The full text of the MIT License under which the project is distributed.
-   `CONTRIBUTING.md`: Provides guidelines for developers who want to contribute to the project, including coding standards and pull request procedures.
-   `CODE_OF_CONDUCT.md`: Outlines the standards for behavior within the community to ensure a welcoming and collaborative environment.
-   `ARCHITECTURE.md`: A high-level document detailing the SDK's design principles, component interactions, and overall architecture.
-   `.gitignore`: Specifies files and directories that should be ignored by Git (e.g., build outputs, IDE files).
-   `.springjavaformatconfig`: A configuration file for the `spring-javaformat` plugin to ensure consistent code style across the project.

### 3.2. GitHub Configuration (`.github/`)

-   `PULL_REQUEST_TEMPLATE.md`: A template that pre-populates the description for new pull requests, ensuring contributors provide necessary context.
-   `ISSUE_TEMPLATE/`: A directory containing templates for bug reports (`bug-report.yml`) and feature requests (`feature-request.yml`), standardizing issue submission.
-   `workflows/`: Contains YAML files defining GitHub Actions for CI/CD.
    -   `build.yml`: Defines the main build and test workflow, triggered on pushes and pull requests to validate the codebase.
    -   `lint-pr.yaml`: A workflow that specifically checks pull requests for code style and formatting issues.
    -   `release.yml`: Automates the process of building, signing, and publishing new releases to Maven Central.

### 3.3. Core SDK Module (`core/`)

This is the primary module containing all the SDK's logic. It gets packaged and deployed to Maven Central as `zai-sdk`.

-   `pom.xml`: The Project Object Model for the `core` module. It declares the SDK's dependencies, including OkHttp, Retrofit, Jackson, RxJava, and Lombok.

#### `core/src/main/java/ai/z/openapi/`

-   `AbstractAiClient.java`: An abstract base class for the AI clients. It manages the lifecycle of services and the underlying HTTP client, providing a common foundation for `ZaiClient` and `ZhipuAiClient`.
-   `ZaiClient.java`: The main client for interacting with the Z.ai API. It uses a fluent builder for configuration and provides access to all the different AI services.
-   `ZhipuAiClient.java`: A specialized client for interacting with the Zhipu AI (BigModel) API, inheriting from `AbstractAiClient`.

##### `api/` Sub-package
This package contains the Retrofit interfaces that define the raw HTTP API endpoints.

-   `agents/AgentsApi.java`: Defines endpoints for the Agents API.
-   `assistant/AssistantApi.java`: Defines endpoints for the Assistants API.
-   `audio/AudioApi.java`: Defines endpoints for audio services (speech-to-text, text-to-speech).
-   `batches/BatchesApi.java`: Defines endpoints for the Batch Processing API.
-   `chat/ChatApi.java`: Defines endpoints for the Chat Completions API (sync, async, streaming).
-   `embedding/EmbeddingApi.java`: Defines endpoints for the Text Embeddings API.
-   `file/FileApi.java`: Defines endpoints for file management (upload, download, list).
-   `images/ImagesApi.java`: Defines endpoints for the Image Generation API.
-   `tools/ToolsApi.java`: Defines endpoints for tool-related functionalities.
-   `videos/VideosApi.java`: Defines endpoints for video processing APIs.
-   `voiceclone/VoiceCloneApi.java`: Defines endpoints for the Voice Cloning API.
-   `web_search/WebSearchApi.java`: Defines endpoints for the Web Search API.

##### `core/` Sub-package
This package contains the foundational and cross-cutting concerns of the SDK.

-   `Constants.java`: A centralized file for all constant values, such as model names, API URLs, and default settings.
-   `cache/`:
    -   `ICache.java`: An interface for a generic cache.
    -   `LocalCache.java`: An in-memory implementation of `ICache`, used for caching authentication tokens.
-   `config/ZaiConfig.java`: A POJO that holds all configuration settings for the client, such as API key, timeouts, and base URL.
-   `model/`:
    -   `ClientRequest.java`, `ClientResponse.java`, `FlowableClientResponse.java`, `BiFlowableClientResponse.java`: Generic wrapper classes that standardize the handling of requests and responses, including support for RxJava's `Flowable` for streaming.
-   `response/HttpxBinaryResponseContent.java`: A class to handle binary responses from the API, such as audio or image files.
-   `token/`:
    -   `TokenManager.java`: An interface for managing API authentication tokens.
    -   `GlobalTokenManager.java`: A singleton implementation of `TokenManager` that handles JWT generation and caching.
    -   `HttpRequestInterceptor.java`: An OkHttp interceptor that automatically adds the `Authorization` header with the correct token to every outgoing request.

##### `service/` Sub-package
This package contains the user-facing services and their associated data models (POJOs/DTOs). Each sub-package corresponds to a specific API feature.

-   `AbstractClientBaseService.java`: A base class for all service implementations, providing access to the configured Retrofit instance.
-   `CommonRequest.java`: A base class for request parameter objects.
-   For each feature (e.g., `agents`, `assistant`, `audio`, `chat`, etc.), the pattern is:
    -   `*Service.java`: An interface defining the high-level service methods (e.g., `AgentService`, `ChatService`).
    -   `*ServiceImpl.java`: The implementation of the service interface, which orchestrates the calls to the `api` interfaces.
    -   A set of POJOs representing the data structures for that feature. For example, in `service/audio/`:
        -   `AudioSpeechRequest.java`: Parameters for a text-to-speech request.
        -   `AudioSpeechResponse.java`: The response from a text-to-speech request.
        -   `AudioTranscriptionRequest.java`: Parameters for a speech-to-text request.
        -   `AudioTranscriptionResponse.java`: The response from a speech-to-text request.
        -   This pattern repeats for all other services, providing strongly-typed objects for every API interaction.

##### `utils/` Sub-package
-   `FlowableRequestSupplier.java`, `RequestSupplier.java`: Functional interfaces used for supplying requests in a reactive chain.
-   `OkHttps.java`: A utility class for building and configuring the `OkHttpClient` instance used by Retrofit.
-   `StringUtils.java`: A utility class with helper methods for handling strings.

#### `core/src/test/`
This directory contains all the tests for the `core` module.
-   The package structure under `src/test/java` mirrors `src/main/java` exactly.
-   `ZaiClientTest.java`, `ZhipuAiClientTest.java`: Tests for the main client classes.
-   Each service has a corresponding test class in `service/` (e.g., `service/chat/ChatServiceTest.java`).
-   `resources/`: Contains test-specific resources like sample files to upload (`document.pdf`, `asr.mp3`), mock responses, and logging configuration (`log4j.properties`).

### 3.4. Samples Module (`samples/`)

This module provides a collection of runnable examples to demonstrate SDK usage. It is not deployed and is intended for developer reference.

-   `pom.xml`: The Maven configuration for the `samples` module, which primarily declares a dependency on the `core` module.
-   `src/main/ai.z.openapi.samples/`: Contains all the example classes.
    -   `AgentExample.java`: Demonstrates using the agent completion API.
    -   `AudioSpeechExample.java`: Shows how to convert text to speech.
    -   `ChatCompletionExample.java`: Basic example of a non-streaming chat conversation.
    -   `ChatCompletionStreamExample.java`: Shows how to handle a streaming chat response.
    -   `ChatCompletionWithCustomHeadersExample.java`: Demonstrates adding custom HTTP headers to a request.
    -   `ClientConfigurationExample.java`: Shows how to use the builder to create a custom-configured client.
    -   `CustomTimeoutExample.java`: Demonstrates setting custom network timeouts.
    -   `EmbeddingsExample.java`: Shows how to generate text embeddings.
    -   `FunctionCallingExample.java`: A detailed example of how to use the function calling feature.
    -   `ImageGenerationExample.java`: Shows how to generate an image from a text prompt.
    -   `ViduTextToVideoExample.java`: Demonstrates generating a video from a text description.
    -   `VoiceCloneExample.java`: Shows how to use the voice cloning API.
    -   `WebSearchExample.java`: Demonstrates using the web search capabilities.
    -   ...and many other examples covering nearly every feature of the SDK.
-   `src/main/resources/grounding.png`: An image file used in one of the vision-related examples.

### 3.5. Script Directory (`script/`)

-   `.note`: A file likely containing development notes or reminders for the maintainers of the project.