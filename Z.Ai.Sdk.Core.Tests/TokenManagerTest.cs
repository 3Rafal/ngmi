using Z.Ai.Sdk.Core.Config;
using Z.Ai.Sdk.Core.Cache;
using Z.Ai.Sdk.Core.Token;

namespace Z.Ai.Sdk.Core.Tests
{
    public class TokenManagerTest
    {
        [Fact]
        public void TestTokenManager()
        {
            // Arrange
            var zAiConfig = new ZaiConfig();
            zAiConfig.WithApiKey("a.abcdefghabcdefghabcdefghabcdefgh");
            ICache cache = LocalCache.GetInstance();
            TokenManager tokenManager = new TokenManager(cache);

            // Act
            string token = tokenManager.GetToken(zAiConfig);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public void TestTokenManagerCache()
        {
            // Arrange
            var zAiConfig = new ZaiConfig();
            zAiConfig.WithApiKey("a.abcdefghabcdefghabcdefghabcdefgh");
            string tokenCacheKey = $"zai_oapi_token-{zAiConfig.ApiKey}";
            ICache cache = LocalCache.GetInstance();
            TokenManager tokenManager = new TokenManager(cache);

            // Act
            string token = tokenManager.GetToken(zAiConfig);

            // Assert
            Assert.NotNull(token);
            Assert.NotNull(cache.Get(tokenCacheKey));
        }
    }
}