using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public class ZaiLogger
{
    static ILoggerFactory _factory = NullLoggerFactory.Instance;

    public static void SetLogger(ILoggerFactory factory)
    {
        _factory = factory;
    }

    public static ILogger GetLogger<T>()
    {
        return _factory.CreateLogger<T>();
    }
}