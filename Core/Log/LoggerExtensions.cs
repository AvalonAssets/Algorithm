using System;

namespace AvalonAssets.Core.Log
{
    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, LogLevel logLevel, string tag, string message)
        {
            logger.Log(logLevel, tag, message, null);
        }

        public static void Assert(this ILogger logger, string tag, string message)
        {
            logger.Assert(tag, message, null);
        }

        public static void Assert(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Log(LogLevel.Assert, tag, message, exception);
        }

        public static void A(this ILogger logger, string tag, string message)
        {
            logger.Assert(tag, message);
        }

        public static void A(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Assert(tag, message, exception);
        }

        public static void Debug(this ILogger logger, string tag, string message)
        {
            logger.Log(LogLevel.Debug, tag, message);
        }

        public static void Debug(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Log(LogLevel.Debug, tag, message, exception);
        }

        public static void D(this ILogger logger, string tag, string message)
        {
            logger.Debug(tag, message);
        }

        public static void D(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Debug(tag, message, exception);
        }

        public static void Error(this ILogger logger, string tag, string message)
        {
            logger.Log(LogLevel.Error, tag, message);
        }

        public static void Error(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Log(LogLevel.Error, tag, message, exception);
        }

        public static void E(this ILogger logger, string tag, string message)
        {
            logger.Error(tag, message);
        }

        public static void E(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Error(tag, message, exception);
        }

        public static void Info(this ILogger logger, string tag, string message)
        {
            logger.Log(LogLevel.Info, tag, message);
        }

        public static void Info(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Log(LogLevel.Info, tag, message, exception);
        }

        public static void I(this ILogger logger, string tag, string message)
        {
            logger.Info(tag, message);
        }

        public static void I(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Info(tag, message, exception);
        }

        public static void Verbose(this ILogger logger, string tag, string message)
        {
            logger.Log(LogLevel.Verbose, tag, message);
        }

        public static void Verbose(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Log(LogLevel.Verbose, tag, message, exception);
        }

        public static void V(this ILogger logger, string tag, string message)
        {
            logger.Verbose(tag, message);
        }

        public static void V(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Verbose(tag, message, exception);
        }

        public static void Warn(this ILogger logger, string tag, string message)
        {
            logger.Log(LogLevel.Warn, tag, message);
        }

        public static void Warn(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Log(LogLevel.Warn, tag, message, exception);
        }

        public static void W(this ILogger logger, string tag, string message)
        {
            logger.Warn(tag, message);
        }

        public static void W(this ILogger logger, string tag, string message, Exception exception)
        {
            logger.Warn(tag, message, exception);
        }
    }
}