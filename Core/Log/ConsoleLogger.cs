using System;

namespace AvalonAssets.Core.Log
{
    internal class ConsoleLogger : ILogger
    {
        public void Log(LogLevel logLevel, string tag, string message, Exception exception)
        {
            Console.Out.WriteLine(exception == null
                ? $"[{logLevel}][{tag}]{message}"
                : $"[{logLevel}][{tag}]{message} - {exception.Message}");
        }
    }
}