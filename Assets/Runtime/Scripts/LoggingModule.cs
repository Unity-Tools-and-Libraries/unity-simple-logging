using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private Dictionary<string, Dictionary<LogType, LoggingLevelConfiguration>> LogContextLevels = new Dictionary<string, Dictionary<LogType, LoggingLevelConfiguration>>()
        {
            { "*", new Dictionary<LogType, LoggingLevelConfiguration>() {
                    {  LogType.Log, new LoggingLevelConfiguration("*", LogType.Log, true, ConsoleLogConsumer.CONSUMER) },
                    { LogType.Error, new LoggingLevelConfiguration("*", LogType.Error, true, ConsoleLogConsumer.CONSUMER) },
                    { LogType.Warning, new LoggingLevelConfiguration("*", LogType.Warning, true, ConsoleLogConsumer.CONSUMER) }
                }
            }
        };

        private Dictionary<string, LogLevel> CachedContextLevels = new Dictionary<string, LogLevel>();

        private void DoLog(LogLevel level, string logMessage, params string[] logContexts)
        {
            if (logContext == null)
            {
                logContext = "*";
            }
            var consumer = LogContextLevels[logContext][level].sink;
            string finalMessage = string.Format("[{0}] {1}", logContext, logMessage);
            consumer.Log(level, finalMessage);
        }

        private string GenerateLogMessage(LogLevel level, string logMessage, string[] logContexts)
        {
            bool logEnabled = false;
            logContext = logContext == null ? "*" : logContext;
            Dictionary<LogType, LoggingLevelConfiguration> levels;
            if (LogContextLevels.TryGetValue(logContext, out levels) || LogContextLevels.TryGetValue(logContext, out levels))
            {
                logEnabled = levels[logLevel].enabled;
            }
            return logEnabled;
        }

        private bool IsLogEnabled(LogLevel logLevel, string logContext)
        {
            logContext = logContext ?? "*";
            var contextTokens = new Stack<string>(logContext.Split("."));
            LogLevel? targetContextLevel = null;
            StringBuilder candidateContext = new StringBuilder();
            do
            {
                for (int i = 0; i < 2; i++) {
                    candidateContext.Clear();
                    candidateContext.AppendJoin(".", contextTokens.Reverse());
                    if(i == 1)
                    {
                        if(candidateContext.Length > 1)
                        {
                            candidateContext.Append(".");
                        }
                        candidateContext.Append("*");
                    }
                    if (LogContextLevels.ContainsKey(candidateContext.ToString()))
                    {
                        targetContextLevel = LogContextLevels[candidateContext.ToString()];
                    }
                    else if (i == 0)
                    {
                        contextTokens.Pop();
                    }
                }
            } while (targetContextLevel == null && contextTokens.Count > 0);
            return logLevel <= targetContextLevel;
        }

        /*
         * Enable or disable logging in the given context.
         */
        public void ConfigureLogging(string logContext, LogType? logLevel, ILogConsumer sink = null)
        {
            logLevel = logLevel.HasValue ? logLevel.Value : LogLevel.Info;
            switch (logLevel)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    DoConfiguration(logContext, LogType.Log, false, sink);
                    DoConfiguration(logContext, LogType.Warning, false, sink);
                    break;
                case LogType.Warning:
                    DoConfiguration(logContext, LogType.Log, false, sink);
                    DoConfiguration(logContext, LogType.Error, true, sink);
                    break;
                case LogType.Log:
                    DoConfiguration(logContext, LogType.Warning, true, sink);
                    DoConfiguration(logContext, LogType.Error, true, sink);
                    break;

            }
            DoConfiguration(logContext, logLevel.Value, true, sink);
        }

        private void DoConfiguration(string logContext, LogType logLevel, bool enabled, ILogConsumer sink)
        {
            Dictionary<LogType, LoggingLevelConfiguration> contexts;
            if (!LogContextLevels.TryGetValue(logContext, out contexts))
            {
                contexts = new Dictionary<LogType, LoggingLevelConfiguration>();
                LogContextLevels[logContext] = contexts;
            }
            if(sink == null)
            {
                sink = ConsoleLogConsumer.CONSUMER;
            }
            contexts[logLevel] = new LoggingLevelConfiguration(logContext, logLevel, enabled, sink);
        }
    }
}