using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace io.github.thisisnozaku.logging
{
    /*
     * This class is a logging library.
     * 
     * It allows for lazy evaluation of messages and fine-grained control over what contexts are logged.
     * 
     */
    public class LoggingModule : ILogger
    {
        private Dictionary<string, LogLevel> LogContextLevels = new Dictionary<string, LogLevel>()
        {
            { "*", LogLevel.Info }
        };

        private void DoLog(LogLevel level, string logMessage, params string[] logContexts)
        {
            string finalMessage = GenerateLogMessage(level, logMessage, logContexts);
            switch (level)
            {
                case LogLevel.Fatal:
                    UnityEngine.Debug.LogAssertion(finalMessage);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(finalMessage);
                    break;
                case LogLevel.Warn:
                    UnityEngine.Debug.LogWarning(finalMessage);
                    break;
                case LogLevel.Debug:
                case LogLevel.Info:
                case LogLevel.Trace:
                    UnityEngine.Debug.Log(finalMessage);
                    break;
            }
        }

        private string GenerateLogMessage(LogLevel level, string logMessage, string[] logContexts)
        {
            return String.Format("{0} {1}",
                String.Join("", logContexts.Select(c => string.Format("[{0}]", c))),
                logMessage);
        }

        private bool IsLogEnabled(LogLevel logLevel, string logContext)
        {
            logContext = logContext ?? "*";
            return logLevel <= LogContextLevels.GetValueOrDefault(logContext, LogContextLevels["*"]);
        }

        /*
         * Enable or disable logging in the given context.
         */
        public void ConfigureLogging(string logContext, LogLevel? logLevel)
        {
            logLevel = logLevel.HasValue ? logLevel.Value : LogLevel.Info;
            switch (logLevel)
            {
                case LogLevel.Fatal:
                case LogLevel.Error:
                    DoConfiguration(logContext, LogLevel.Info, false);
                    DoConfiguration(logContext, LogLevel.Warn, false);
                    break;
                case LogLevel.Warn:
                    DoConfiguration(logContext, LogLevel.Info, false);
                    DoConfiguration(logContext, LogLevel.Error, true);
                    break;
                case LogLevel.Info:
                    DoConfiguration(logContext, LogLevel.Warn, true);
                    DoConfiguration(logContext, LogLevel.Error, true);
                    break;

            }
            DoConfiguration(logContext, logLevel.Value, true);
        }

        private void DoConfiguration(string logContext, LogLevel logLevel, bool enabled)
        {
            LogContextLevels[logContext] = logLevel;
        }

        /*
         * Log the given message at the given level, with the given context.
         * 
         * 
         */
        public void Log(LogLevel logType, string logMessage, params string[] logContexts)
        {
            if (logContexts.Length == 0)
            {
                logContexts = new string[]
                {
                    "*"
                };
            }
            var enabledContexts = logContexts.Where(ctx => IsLogEnabled(logType, ctx)).ToArray();
            if (enabledContexts.Length > 0)
            {
                DoLog(logType, logMessage, enabledContexts);
            }
        }

        /*
         * Alternative to the basic log function which uses lazy evaluation when creating the message.
         *
         * The primary use case for this is when strings make use of runtime formatting, as this results in lots 
         * of memory usage and you want to ensure you will actually use the message before creating it.
         */
        public void Log(LogLevel logType, Func<string> messageGenerator, params string[] logContexts)
        {
            if (logContexts.Length == 0)
            {
                logContexts = new string[]
                {
                    "*"
                };
            }
            var enabledContexts = logContexts.Where(ctx => IsLogEnabled(logType, ctx)).ToArray();
            if (enabledContexts.Length > 0)
            {
                DoLog(logType, messageGenerator(), enabledContexts);
            }
        }

        public void Fatal(string logMessage, params string[] logContexts)
        {
            Log(LogLevel.Fatal, logMessage, logContexts);
        }

        public void Fatal(Func<string> messageGenerator, params string[] logContexts)
        {
            Log(LogLevel.Fatal, messageGenerator, logContexts);
        }

        public void Error(string logMessage, params string[] logContexts)
        {
            Log(LogLevel.Error, logMessage, logContexts);
        }

        public void Error(Func<string> messageGenerator, params string[] logContexts)
        {
            Log(LogLevel.Error, messageGenerator, logContexts);
        }

        public void Warn(string logMessage, params string[] logContexts)
        {
            Log(LogLevel.Warn, logMessage, logContexts);
        }

        public void Warn(Func<string> messageGenerator, params string[] logContexts)
        {
            Log(LogLevel.Warn, messageGenerator, logContexts);
        }

        public void Info(string logMessage, params string[] logContexts)
        {
            Log(LogLevel.Info, logMessage, logContexts);
        }

        public void Info(Func<string> messageGenerator, params string[] logContexts)
        {
            Log(LogLevel.Info, messageGenerator, logContexts);
        }

        public void Debug(string logMessage, params string[] logContexts)
        {
            Log(LogLevel.Debug, logMessage, logContexts);
        }

        public void Debug(Func<string> messageGenerator, params string[] logContexts)
        {
            Log(LogLevel.Info, messageGenerator, logContexts);
        }

        public void Trace(string logMessage, params string[] logContexts)
        {
            Log(LogLevel.Trace, logMessage, logContexts);
        }

        public void Trace(Func<string> messageGenerator, params string[] logContexts)
        {
            Log(LogLevel.Trace, messageGenerator, logContexts);
        }

        public void Log(LogType unityLogType, string logMessage, params string[] logContexts)
        {
            switch (unityLogType)
            {
                case LogType.Assert:
                    Log(LogLevel.Fatal, logMessage, logContexts);
                    break;
                case LogType.Error:
                case LogType.Exception:
                    Log(LogLevel.Error, logMessage, logContexts);
                    break;
                case LogType.Log:
                    Log(LogLevel.Info, logMessage, logContexts);
                    break;
                case LogType.Warning:
                    Log(LogLevel.Warn, logMessage, logContexts);
                    break;
            }
        }
    }
}