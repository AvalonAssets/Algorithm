namespace AvalonAssets.Core.Log
{
    public static class Loggers
    {
        private static readonly Lazy<ConsoleLogger> ConsoleLogger = new Lazy<ConsoleLogger>();
        public static ILogger Default => ConsoleLogger.Value;
    }
}