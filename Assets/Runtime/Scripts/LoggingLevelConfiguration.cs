using UnityEngine;
using System.Collections;
using System;

namespace io.github.thisisnozaku.logging
{
	public struct LoggingLevelConfiguration
	{
        public readonly string LogContext;
        public readonly LogType LogType;
        public readonly bool enabled;
        public readonly ILogConsumer sink;

        public LoggingLevelConfiguration(string logContext, LogType logType, bool enabled, ILogConsumer sink)
        {
            LogContext = logContext;
            LogType = logType;
            this.enabled = enabled;
            this.sink = sink;
        }
    }
}