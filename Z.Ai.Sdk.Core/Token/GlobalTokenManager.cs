using Z.Ai.Sdk.Core.Cache;

namespace Z.Ai.Sdk.Core.Token;

/// <summary>
/// Global token manager providing singleton access to TokenManager instance. Uses
/// LocalCache as default cache implementation.
/// </summary>
public static class GlobalTokenManager
{
    private static volatile TokenManager? _globalTokenManager;

    /// <summary>
    /// Gets the global TokenManager instance for V4 API.
    /// </summary>
    /// <returns>TokenManager instance</returns>
    public static TokenManager GetTokenManagerV4()
    {
        if (_globalTokenManager == null)
        {
            lock (typeof(GlobalTokenManager))
            {
                if (_globalTokenManager == null)
                {
                    _globalTokenManager = new TokenManager(LocalCache.GetInstance());
                }
            }
        }
        return _globalTokenManager;
    }

    /// <summary>
    /// Sets custom TokenManager implementation.
    /// </summary>
    /// <param name="tokenManager">Custom TokenManager instance</param>
    public static void SetTokenManager(TokenManager tokenManager)
    {
        _globalTokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
    }

    /// <summary>
    /// Resets the global token manager to default implementation.
    /// This is primarily used for testing purposes.
    /// </summary>
    public static void Reset()
    {
        lock (typeof(GlobalTokenManager))
        {
            _globalTokenManager = null;
        }
    }
}