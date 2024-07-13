using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.github.thisisnozaku.logging
{
    public interface ILogger
    {
        void Log(LogLevel unityLogType, string logMessage, params string[] logContext);
        void Log(LogLevel unityLogType, Func<string> logMessage, params string[] logContext);
    }
}