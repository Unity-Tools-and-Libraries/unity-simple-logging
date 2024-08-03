using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace io.github.thisisnozaku.logging
{
	public struct LoggingLevelConfiguration
	{
        public readonly string LogContext;
        public readonly LogLevel LogType;
        public readonly bool enabled;
        public readonly ILogConsumer[] sinks;

        public LoggingLevelConfiguration(string logContext, LogLevel logType, bool enabled, ILogConsumer sink) : this(logContext, logType, enabled, new ILogConsumer[] { sink })
        {

        }

        public LoggingLevelConfiguration(string logContext, LogLevel logType, bool enabled, ILogConsumer[] sinks)
        {
            LogContext = logContext;
            LogType = logType;
            this.enabled = enabled;
            this.sinks = sinks;
        }

        public override string ToString()
        {
            return $"[{LogContext}]<{LogType}>{enabled} => {string.Join(", ", sinks.Select(_ => _.ToString()))}";
        }
    }
}