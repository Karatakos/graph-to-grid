using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dungen;

public static class SimpleLogger {
    private static string _category = "DungenLib";
    private static ILogger _logger;

    public static ILogger Logger {
        get {
            if (_logger == null)
                _logger = NullLoggerFactory.Instance.CreateLogger(_category);

            return _logger;
        }
    }

    public static ILogger InitializeLogger(ILoggerFactory factory) {
        _logger = factory.CreateLogger(_category);

        return _logger;
    }

}