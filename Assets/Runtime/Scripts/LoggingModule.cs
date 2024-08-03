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
        private Dictionary<string, LoggingLevelConfiguration> LogContextLevels = new Dictionary<string, LoggingLevelConfiguration>()
        {
            { "*", new LoggingLevelConfiguration("*", LogLevel.Trace, true, ConsoleLogConsumer.CONSUMER) }
        };

        private Dictionary<string, LogType> CachedContextLevels = new Dictionary<string, LogType>();

        private void DoLog(LogLevel level, string logMessage, params string[] logContexts)
        {
            if(logContexts.Length == 0)
            {
                logContexts = new string[]
                {
                    "*"
                };
            }
            foreach (var logContext in logContexts)
            {
                foreach (var sink in LogContextLevels[logContext].sinks) {
                    var consumer = sink;
                    string finalMessage = string.Format("[{0}] {1}", logContext, logMessage);
                    consumer.Log(level, finalMessage);
                }
            }
        }

        private string GenerateLogMessage(LogType level, string logMessage, string[] logContexts)
        {
            return $"{level}{string.Join("", logContexts.Select(_ => $"[{_}]"))}{logMessage}";
        }

        private bool IsLogEnabled(LogType logLevel, string logContext)
        {
            logContext = logContext ?? "*";
            var contextTokens = new Stack<string>(logContext.Split("."));
            LogType? targetContextLevel = null;
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
                        //targetContextLevel = new Nullable<LogType>(LogContextLevels[candidateContext.ToString()]);
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
        public void ConfigureLogging(string logContext, LogLevel logLevel, params ILogConsumer[] sinks)
        {
            DoConfiguration(logContext, logLevel, true, sinks);
        }

        private void DoConfiguration(string logContext, LogLevel logLevel, bool enabled, params ILogConsumer[] sinks)
        {
            if(sinks.Length == 0)
            {
                sinks = new ILogConsumer[] { ConsoleLogConsumer.CONSUMER };
            }
            LogContextLevels[logContext] = new LoggingLevelConfiguration(logContext, logLevel, enabled, sinks);
        }

        private LoggingLevelConfiguration GetConfiguration(string context)
        {
            var levels = new Stack<string>(context.Split("."));
            while (levels.Count > 0) {
                for (int i = 0; i < 2; i++)
                {
                    var candidateContext = string.Join(".", levels.Reverse());
                    LoggingLevelConfiguration configuration;
                    if (LogContextLevels.TryGetValue(candidateContext, out configuration))
                    {
                        return configuration;
                    }
                    else
                    {
                        levels.Pop();
                        if(i == 0)
                        {
                            levels.Push("*");
                        }
                    }
                }
            }
            throw new Exception();
        }

        public void Log(LogLevel logLevel, string logMessage, params string[] logContext)
        {
            if(logContext.Length == 0)
            {
                logContext = new string[] { "*" };
            }
            foreach(var context in logContext)
            {
                var config = GetConfiguration(context);
                if (config.LogType >= logLevel && config.enabled)
                {
                    foreach(var sink in config.sinks)
                    {
                        sink.Log(logLevel, FormatMessage(context, logMessage));
                    }
                }
            }
        }

        public void Log(LogLevel logLevel, Func<string> messageGenerator, params string[] logContext)
        {
            if (logContext.Length == 0)
            {
                logContext = new string[] { "*" };
            }
            foreach (var context in logContext)
            {
                var config = GetConfiguration(context);
                if (config.enabled)
                {
                    foreach (var sink in config.sinks)
                    {
                        sink.Log(logLevel, FormatMessage(context, messageGenerator()));
                    }
                }
            }
        }

        private string FormatMessage(string logContext, string message)
        {
            return $"[{logContext}] {message}";
        }
    }
}