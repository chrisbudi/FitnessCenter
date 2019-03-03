using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Log
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger log, Func<string> getMessage)
        {
            if (!log.IsDebugEnabled)
            {
                return;
            }

            var logMessage = getMessage();
            log.Debug(logMessage);
        }

        public static void ThreadContextIDTable(this ILogger log, string ThreadValue)
        {
            log.PushThread("IdTable", ThreadValue);
        }

        public static void ThreadContextNamaTable(this ILogger log, string ThreadValue)
        {
            log.PushThread("NamaTable", ThreadValue);
        }

        public static void ThreadContextNamaTransaksi(this ILogger log, string ThreadValue)
        {
            log.PushThread("NamaTransaksi", ThreadValue);
        }

        public static void Info(this ILogger log, Func<string> getMessage)
        {
            if (!log.IsInfoEnabled)
            {
                return;
            }

            var logMessage = getMessage();
            log.Info(logMessage);
        }



        public static void Warn(this ILogger log, Func<string> getMessage)
        {
            if (!log.IsWarnEnabled)
            {
                return;
            }

            var logMessage = getMessage();
            log.Warn(logMessage);
        }

        public static void Error(this ILogger log, Func<string> getMessage)
        {
            if (!log.IsErrorEnabled)
            {
                return;
            }

            var logMessage = getMessage();
            log.Error(logMessage);
        }

        public static void Fatal(this ILogger log, Func<string> getMessage)
        {
            if (!log.IsFatalEnabled)
            {
                return;
            }

            var logMessage = getMessage();
            log.Fatal(logMessage);
        }
    }
}
