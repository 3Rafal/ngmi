namespace Z.Ai.Sdk.Core;

/// <summary>
/// Constants class containing all the configuration values and model identifiers used
/// throughout the Z.AI OpenAPI SDK.
///
/// This class provides centralized access to:
/// - API base URLs
/// - Model identifiers for different AI capabilities
/// - Invocation method constants
/// </summary>
public static class Constants
{
    // =============================================================================
    // API Configuration
    // =============================================================================

    /// <summary>
    /// Base URL for the ZHIPU AI OpenAPI service. All API requests will be made to
    /// endpoints under this base URL.
    /// </summary>
    public const string ZhipuAiBaseUrl = "https://open.bigmodel.cn/api/paas/v4";

    /// <summary>
    /// Base URL for the Z.AI OpenAPI service. All API requests will be made to endpoints
    /// under this base URL.
    /// </summary>
    public const string ZAiBaseUrl = "https://api.z.ai/api/paas/v4";

    // =============================================================================
    // Text Generation Models
    // =============================================================================

    /// <summary>
    /// GLM-4.5 model code
    /// </summary>
    public const string ModelChatGlm4_5 = "glm-4.5";

    /// <summary>
    /// GLM-4.5V model code
    /// </summary>
    public const string ModelChatGlm4_5V = "glm-4.5v";

    /// <summary>
    /// GLM-4.5-Air model code
    /// </summary>
    public const string ModelChatGlm4_5Air = "glm-4.5-air";

    /// <summary>
    /// GLM-4.5-X model code
    /// </summary>
    public const string ModelChatGlm4_5X = "glm-4.5-x";

    /// <summary>
    /// GLM-4.5-AirX model code
    /// </summary>
    public const string ModelChatGlm4_5AirX = "glm-4.5-airx";

    /// <summary>
    /// GLM-4 Plus model - Enhanced version with improved capabilities.
    /// </summary>
    public const string ModelChatGlm4Plus = "glm-4-plus";

    /// <summary>
    /// GLM-4 Air model - Lightweight version optimized for speed.
    /// </summary>
    public const string ModelChatGlm4Air = "glm-4-air";

    /// <summary>
    /// GLM-4 Flash model - Ultra-fast response model.
    /// </summary>
    public const string ModelChatGlm4Flash = "glm-4-flash";

    /// <summary>
    /// GLM-4 standard model - Balanced performance and capability.
    /// </summary>
    public const string ModelChatGlm4 = "glm-4";

    /// <summary>
    /// GLM-4 model version 0520 - Specific version release.
    /// </summary>
    public const string ModelChatGlm4_0520 = "glm-4-0520";

    /// <summary>
    /// GLM-4 AirX model - Extended Air model with additional features.
    /// </summary>
    public const string ModelChatGlm4Airx = "glm-4-airx";

    /// <summary>
    /// GLM-4 Long model - Optimized for long-context conversations.
    /// </summary>
    public const string ModelChatGlmLong = "glm-4-long";

    /// <summary>
    /// GLM-4 Voice model - Specialized for voice-related tasks.
    /// </summary>
    public const string ModelChatGlm4Voice = "glm-4-voice";

    /// <summary>
    /// GLM-4.1V Thinking Flash model - Visual reasoning model with thinking capabilities.
    /// </summary>
    public const string ModelChatGlm4_1VThinkingFlash = "glm-4.1v-thinking-flash";

    /// <summary>
    /// GLM-Z1 Air model - Optimized for mathematical and logical reasoning.
    /// </summary>
    public const string ModelChatGlmZ1Air = "glm-z1-air";

    /// <summary>
    /// GLM-Z1 AirX model - Fastest domestic inference model with 200 tokens/s.
    /// </summary>
    public const string ModelChatGlmZ1AirX = "glm-z1-airx";

    /// <summary>
    /// GLM-Z1 Flash model - Completely free reasoning model service.
    /// </summary>
    public const string ModelChatGlmZ1Flash = "glm-z1-flash";

    /// <summary>
    /// GLM-4 Air 250414 model - Enhanced with reinforcement learning optimization.
    /// </summary>
    public const string ModelChatGlm4Air250414 = "glm-4-air-250414";

    /// <summary>
    /// GLM-4 Flash 250414 model - Latest free language model.
    /// </summary>
    public const string ModelChatGlm4Flash250414 = "glm-4-flash-250414";

    /// <summary>
    /// GLM-4 FlashX model - Enhanced Flash version with ultra-fast inference speed.
    /// </summary>
    public const string ModelChatGlm4FlashX = "glm-4-flashx";

    /// <summary>
    /// GLM-4 9B model - Open-source model with 9 billion parameters.
    /// </summary>
    public const string ModelChatGlm4_9B = "glm-4-9b";

    /// <summary>
    /// GLM-4 Assistant model - AI assistant for various business scenarios.
    /// </summary>
    public const string ModelChatGlm4Assistant = "glm-4-assistant";

    /// <summary>
    /// GLM-4 AllTools model - Agent model for complex task planning and execution.
    /// </summary>
    public const string ModelChatGlm4AllTools = "glm-4-alltools";

    /// <summary>
    /// ChatGLM3 6B model - Open-source base model with 6 billion parameters.
    /// </summary>
    public const string ModelChatGlm3_6B = "chatglm3-6b";

    /// <summary>
    /// CodeGeeX-4 model - Code generation and completion model.
    /// </summary>
    public const string ModelCodeGeeX4 = "codegeex-4";

    // =============================================================================
    // Audio Speech Recognition Models
    // =============================================================================

    /// <summary>
    /// GLM-ASR model - Context-aware audio transcription model that converts audio to
    /// fluent and readable text. Supports Chinese, English, and various Chinese dialects.
    /// Improved performance in noisy environments.
    /// </summary>
    public const string ModelGlmAsr = "glm-asr";

    // =============================================================================
    // Real-time Interaction Models
    // =============================================================================

    /// <summary>
    /// GLM-Realtime Air model - Real-time video call model with cross-modal reasoning
    /// capabilities across text, audio, and video. Supports real-time interruption.
    /// </summary>
    public const string ModelGlmRealtimeAir = "glm-realtime-air";

    /// <summary>
    /// GLM-Realtime Flash model - Fast real-time video call model with cross-modal
    /// reasoning capabilities. Supports camera interaction and screen sharing.
    /// </summary>
    public const string ModelGlmRealtimeFlash = "glm-realtime-flash";

    // =============================================================================
    // Vision Models (Image Understanding)
    // =============================================================================

    /// <summary>
    /// GLM-4V Plus model - Enhanced vision model for image understanding.
    /// </summary>
    public const string ModelChatGlm4VPlus = "glm-4v-plus";

    /// <summary>
    /// GLM-4V standard model - Standard vision model for image analysis.
    /// </summary>
    public const string ModelChatGlm4V = "glm-4v";

    /// <summary>
    /// GLM-4V Plus 0111 model - Variable resolution video and image understanding.
    /// </summary>
    public const string ModelChatGlm4VPlus0111 = "glm-4v-plus-0111";

    /// <summary>
    /// GLM-4.1V-Thinking-FlashX Variable resolution video and image understanding.
    /// </summary>
    public const string ModelChatGlm4ThinkingFlashX = "glm-4.1v-thinking-flashx";

    /// <summary>
    /// GLM-4V Flash model - Free and powerful image understanding model.
    /// </summary>
    public const string ModelChatGlm4VFlash = "glm-4v-flash";

    // =============================================================================
    // Image Generation Models
    // =============================================================================

    /// <summary>
    /// CogView-3 Plus model - Enhanced image generation capabilities.
    /// </summary>
    public const string ModelCogView3Plus = "cogview-3-plus";

    /// <summary>
    /// CogView-3 standard model - Standard image generation model.
    /// </summary>
    public const string ModelCogView3 = "cogview-3";

    /// <summary>
    /// CogView-3 Flash model - Free image generation model.
    /// </summary>
    public const string ModelCogView3Flash = "cogview-3-flash";

    /// <summary>
    /// CogView-4 250304 model - Advanced image generation with text capabilities.
    /// </summary>
    public const string ModelCogView4_250304 = "cogview-4-250304";

    /// <summary>
    /// CogView-4 model - Advanced image generation for precise and personalized AI image
    /// expression.
    /// </summary>
    public const string ModelCogView4 = "cogview-4";

    // =============================================================================
    // Video Generation Models
    // =============================================================================

    /// <summary>
    /// CogVideoX model - Video generation from text or images.
    /// </summary>
    public const string ModelCogVideoX = "cogvideox";

    /// <summary>
    /// CogVideoX Flash model - Free video generation model.
    /// </summary>
    public const string ModelCogVideoXFlash = "cogvideox-flash";

    /// <summary>
    /// CogVideoX-2 model - New video generation model.
    /// </summary>
    public const string ModelCogVideoX2 = "cogvideox-2";

    /// <summary>
    /// CogVideoX-3 model
    /// </summary>
    public const string ModelCogVideoX3 = "cogvideox-3";

    /// <summary>
    /// Vidu Q1 Text model - High-performance video generation from text input. Supports
    /// general and anime styles.
    /// </summary>
    public const string ModelViduQ1Text = "viduq1-text";

    /// <summary>
    /// Vidu Q1 Image model - Video generation from first frame image and text description.
    /// </summary>
    public const string ModelViduQ1Image = "viduq1-image";

    /// <summary>
    /// Vidu Q1 Start-End model - Video generation from first and last frame images.
    /// </summary>
    public const string ModelViduQ1StartEnd = "viduq1-start-end";

    /// <summary>
    /// Vidu 2 Image model - Enhanced video generation from first frame image and text
    /// description.
    /// </summary>
    public const string ModelVidu2Image = "vidu2-image";

    /// <summary>
    /// Vidu 2 Start-End model - Enhanced video generation from first and last frame
    /// images.
    /// </summary>
    public const string ModelVidu2StartEnd = "vidu2-start-end";

    /// <summary>
    /// Vidu 2 Reference model - Video generation with reference images of people, objects,
    /// etc.
    /// </summary>
    public const string ModelVidu2Reference = "vidu2-reference";

    // =============================================================================
    // Embedding Models
    // =============================================================================

    /// <summary>
    /// Embedding model version 2 - Text embedding generation.
    /// </summary>
    public const string ModelEmbedding2 = "embedding-2";

    /// <summary>
    /// Embedding model version 3 - Latest text embedding generation.
    /// </summary>
    public const string ModelEmbedding3 = "embedding-3";

    // =============================================================================
    // Specialized Models
    // =============================================================================

    /// <summary>
    /// CharGLM-3 model - Anthropomorphic character interaction model.
    /// </summary>
    public const string ModelCharGlm3 = "charglm-3";

    /// <summary>
    /// CogTTS model - Text-to-Speech synthesis model.
    /// </summary>
    public const string ModelTts = "cogtts";

    /// <summary>
    /// Rerank model - Text reordering and relevance scoring.
    /// </summary>
    public const string ModelRerank = "rerank";
}
