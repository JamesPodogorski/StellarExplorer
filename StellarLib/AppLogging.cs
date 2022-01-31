using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace StellarLib;

public class AppLogging
{
    private static ILoggerFactory? _loggerFactory { get; set; } = null;

    public AppLogging(ILoggerFactory factory)
    {
        if (AppLogging._loggerFactory == null)
        {
            AppLogging._loggerFactory = factory;
        }
    }

    public static ILogger<T>? CreateLogger<T>()
    {
       _ = _loggerFactory ?? throw new NullReferenceException("ILoggerFactory has not been injected");

       return _loggerFactory.CreateLogger<T>();
    }
}