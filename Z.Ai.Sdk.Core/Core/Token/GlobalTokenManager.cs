using Z.Ai.Sdk.Core.Cache;

namespace Z.Ai.Sdk.Core.Token;

/// <summary>
/// Global token manager providing singleton access to TokenManager instance. Uses
/// LocalCache as default cache implementation.
/// </summary>
public static class GlobalTokenManager
{
    private static volatile TokenManager _globalTokenManager = new(LocalCache.GetInstance());

    /// <summary>
    /// Gets the global TokenManager instance for V4 API.
    /// </summary>
    /// <returns>TokenManager instance</returns>
    public static TokenManager GetTokenManagerV4() => _globalTokenManager;

    /// <summary>
    /// Sets custom TokenManager implementation.
    /// </summary>
    /// <param name="tokenManager">Custom TokenManager instance</param>
    public static void SetTokenManager(TokenManager tokenManager) => _globalTokenManager = tokenManager;
}