using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.github.thisisnozaku.logging
{
    public interface ILogger
    {
        void Log(LogType unityLogType, string logMessage, params string[] logContext);
        void Log(LogLevel logLevel, string logMessage, params string[] logContext);
        void Log(LogLevel logLevel, Func<string> messageGenerator, params string[] logContext);
        void Fatal(string logMessage, params string[] logContext);
        void Fatal(Func<string> messageGenerator, params string[] logContext);
        void Error(string logMessage, params string[] logContext);
        void Error(Func<string> messageGenerator, params string[] logContext);
        void Warn(string logMessage, params string[] logContext);
        void Warn(Func<string> messageGenerator, params string[] logContext);
        void Info(string logMessage, params string[] logContext);
        void Info(Func<string> messageGenerator, params string[] logContext);
        void Debug(string logMessage, params string[] logContext);
        void Debug(Func<string> messageGenerator, params string[] logContext);
        void Trace(string logMessage, params string[] logContext);
        void Trace(Func<string> messageGenerator, params string[] logContext);
    }
}