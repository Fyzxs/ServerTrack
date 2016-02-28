/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;
using log4net;

namespace ServerTrack.Log
{
    /// <summary>
    /// A Logging Helper to enable lazy processing of the logging message.
    /// Traditional logging will still construct the message at runtime to be 
    /// logged; even if it won't be logged due to the LogLevel setting.
    /// These extension methods enable the use of Lambda's to enabled delayed 
    /// evaluation of the string construction to only the logs that will be written.
    /// 
    /// A small optimization; but given it's normally with strings - a non-mutable object
    /// it can have an unexpected significant performance hit for a high impact system.
    /// </summary>
    public static class LogHelper
    {
        public static void Debug(this ILog log, Func<object> messageProvider)
        {
            if (log.IsDebugEnabled) { log.Debug(messageProvider()); }
        }

        public static void Info(this ILog log, Func<object> messageProvider)
        {
            if (log.IsInfoEnabled) { log.Info(messageProvider()); }
        }

        public static void Warn(this ILog log, Func<object> messageProvider)
        {
            if (log.IsWarnEnabled) { log.Warn(messageProvider()); }
        }

        public static void Error(this ILog log, Func<object> messageProvider)
        {
            if (log.IsErrorEnabled) { log.Error(messageProvider()); }
        }

        public static void Error(this ILog log, Func<object> messageProvider, Exception ex)
        {
            if (log.IsErrorEnabled) { log.Error(messageProvider(), ex); }
        }

        public static void Fatal(this ILog log, Func<object> messageProvider)
        {
            if (log.IsFatalEnabled) { log.Fatal(messageProvider()); }
        }

        public static void Fatal(this ILog log, Func<object> messageProvider, Exception ex)
        {
            if (log.IsFatalEnabled) { log.Fatal(messageProvider(), ex); }
        }
    }
}