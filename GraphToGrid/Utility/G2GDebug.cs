namespace GraphToGrid;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public static class G2GDebug {
    private static ILogger _logger = null;

    public static ILogger Logger {
        get {
            if (_logger == null)
                _logger = NullLoggerFactory.Instance.CreateLogger("GraphToGrid");

            return _logger;
        }

        set { _logger = value; }
    }

    public static void Write(string str) {
        Write(LogLevel.Debug, str);
    }

    public static void Write(LogLevel level, string str) {
        Logger.Log(level, str);
    }
}