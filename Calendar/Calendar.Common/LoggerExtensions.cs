using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Calendar.Common
{
    public static class LoggerExtensions
    {
        public static void LogW<T>(this ILogger<T> logger, Exception ex, string format,
            [CallerMemberName] string parentMethodName = "", params object[] args)
        {
            logger.LogWarning(ex, $"{parentMethodName} - Parameters:{format}", args);
        }
        public static void LogE<T>(this ILogger<T> logger, Exception ex, string format,
            [CallerMemberName] string parentMethodName = "", params object[] args)
        {
            logger.LogError(ex, $"{parentMethodName} - Parameters:{format}", args);
        }
        public static void LogI<T>(this ILogger<T> logger, Exception ex, string format,
            [CallerMemberName] string parentMethodName = "", params object[] args)
        {
            logger.LogInformation(ex, $"{parentMethodName} - Parameters:{format}", args);
        }
        public static void LogD<T>(this ILogger<T> logger, Exception ex, string format,
            [CallerMemberName] string parentMethodName = "", params object[] args)
        {
            logger.LogDebug(ex, $"{parentMethodName} - Parameters:{format}", args);
        }
        public static void LogT<T>(this ILogger<T> logger, Exception ex, string format,
            [CallerMemberName] string parentMethodName = "", params object[] args)
        {
            logger.LogTrace(ex, $"{parentMethodName} - Parameters:{format}", args);
        }
        public static void LogC<T>(this ILogger<T> logger, Exception ex, string format,
            [CallerMemberName] string parentMethodName = "", params object[] args)
        {
            logger.LogCritical(ex, $"{parentMethodName} - Parameters:{format}", args);
        }
    }
}