using System;

namespace AvalonAssets.Core.Log
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, string tag,  string message, Exception exception);
    }
}