using System;
using UnityEngine;

namespace io.github.thisisnozaku.logging {
    /**
     * 
     */
    public class ConsoleLogConsumer : ILogConsumer
    {
        public static ConsoleLogConsumer CONSUMER = new ConsoleLogConsumer();
        public void Log(LogLevel level, string message)
        {
            var finalMessage = $"{message}";
            switch (level) {
                case LogLevel.Fatal:
                case LogLevel.Error:
                    Debug.LogError(finalMessage);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarning(finalMessage);
                    break;
                default:
                    Debug.Log(finalMessage);
                    break;
            }
        }
    }
}

