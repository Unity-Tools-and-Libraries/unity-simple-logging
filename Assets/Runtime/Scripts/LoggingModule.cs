using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.github.thisisnozaku.logging
{
    /*
     * This class is a logging library.
     * 
     * It allows for lazy evaluation of messages and fine-grained control over what contexts are logged.
     * 
     * The log levels of Log (for informational message), Warning (for messages which are not errors but 
     * indicate potential problems) and Error(for error messages) are used. Assert and Exception are 
     * treated as equivalent to Error.
     * 
     * 
     */
    public class LoggingModule
    {
        private Dictionary<string, Dictionary<LogType, bool>> LogContextLevels = new Dictionary<string, Dictionary<LogType, bool>>()
        {
            { "*", new Dictionary<LogType, bool>() { {  LogType.Log, true} } }
        };

        private void DoLog(LogType level, string logMessage, string logContext = null)
        {
            if(logContext == null)
            {
                logContext = "*";
            }
            string finalMessage = string.Format("[{0}] {1}", logContext, logMessage);
            switch (level)
            {
                case LogType.Error:
                case LogType.Exception:
                    Debug.LogError(finalMessage);
                    break;
                case LogType.Log:
                    Debug.Log(finalMessage);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(finalMessage);
                    break;
            }
        }

        private bool IsLogEnabled(LogType logLevel, string logContext)
        {
            bool logEnabled = false;
            logContext = logContext == null ? "*" : logContext;
            Dictionary<LogType, bool> levels;
            if (LogContextLevels.TryGetValue(logContext, out levels) || LogContextLevels.TryGetValue(logContext, out levels))
            {
                switch(logLevel)
                {
                    case LogType.Assert:
                    case LogType.Error:
                    case LogType.Exception:
                        if (!levels.TryGetValue(LogType.Log, out logEnabled))
                        {
                            if (!levels.TryGetValue(LogType.Warning, out logEnabled))
                            {
                                levels.TryGetValue(LogType.Error, out logEnabled);
                            }
                        }
                        break;
                    case LogType.Warning:
                        if(!levels.TryGetValue(LogType.Warning, out logEnabled))
                        {
                            levels.TryGetValue(LogType.Error, out logEnabled);
                        }
                        break;
                    case LogType.Log:
                        levels.TryGetValue(logLevel, out logEnabled);
                        break;
                    
                }
            }
            return logEnabled;
        }
        /*
         * Log the given message at the given level, with the given context.
         * 
         * 
         */
        public void Log(LogType logType, string logMessage, string logContext = null)
        {   
            if (IsLogEnabled(logType, logContext))
            {
                DoLog(logType, logMessage, logContext);
            }
        }
        /*
         * Alternative to the basic log function which uses lazy evaluation when creating the message.
         *
         * The primary use case for this is when strings make use of runtime formatting, as this results in lots 
         * of memory usage and you want to ensure you will actually use the message before creating it.
         */
        public void Log(LogType logType, Func<string> logMessageGenerator, string logContext = null)
        {
            if (IsLogEnabled(logType, logContext))
            {
                DoLog(logType, logMessageGenerator(), logContext);
            }
        }

        public void Log(string message, string logContext = null)
        {
            Log(LogType.Log, message, logContext);
        }

        public void Log(Func<string> messageGenerator, string logContext = null)
        {
            Log(LogType.Log, messageGenerator, logContext);
        }

        public void Warning(string message, string logContext = null)
        {
            Log(LogType.Warning, message, logContext);
        }

        public void Warning(Func<string> messageGenerator, string logContext = null)
        {
            Log(LogType.Warning, messageGenerator, logContext);
        }

        public void Exception(string message, string logContext = null)
        {
            Log(LogType.Exception, message, logContext);
        }

        public void Exception(Func<string> messageGenerator, string logContext = null)
        {
            Log(LogType.Exception, messageGenerator, logContext);
        }

        /*
         * Enable or disable logging in the given context.
         */
        public void ConfigureLogging(string logContext, LogType? logLevel, bool enabled = true)
        {
            logLevel = logLevel.HasValue ? logLevel.Value : LogType.Log;
            Dictionary<LogType, bool> contexts;
            switch (logLevel)
            {
                case LogType.Log:
                    ConfigureLogging(logContext, LogType.Warning, enabled);
                    break;
                case LogType.Warning:
                    ConfigureLogging(logContext, LogType.Error, enabled);
                    break;
            }
            if (!LogContextLevels.TryGetValue(logContext, out contexts))
            {
                contexts = new Dictionary<LogType, bool>();
                LogContextLevels[logContext] = contexts;
            }
            if (!logLevel.HasValue)
            {
                contexts.Clear();
            }
            else
            {
                contexts[logLevel.Value] = enabled;
            }
        }
    }
}