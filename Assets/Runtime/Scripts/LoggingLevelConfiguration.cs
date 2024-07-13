using UnityEngine;
using System.Collections;
using System;

namespace io.github.thisisnozaku.logging
{
	public struct LoggingLevelConfiguration
	{
        public readonly string LogContext;
        public readonly LogLevel LogType;
        public readonly bool enabled;
        public readonly ILogConsumer sink;

        public LoggingLevelConfiguration(string logContext, LogLevel logType, bool enabled, ILogConsumer sink)
        {
            LogContext = logContext;
            LogType = logType;
            this.enabled = enabled;
            this.sink = sink;
        }

        public override string ToString()
        {
            return $"[{LogContext}]<{LogType}>{enabled} => {sink}";
        }
    }
}