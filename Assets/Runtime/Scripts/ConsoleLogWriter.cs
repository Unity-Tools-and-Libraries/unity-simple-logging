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
            switch(level){
                default:
                    Debug.Log(message);
                    break;
                case LogLevel.Error:
                case LogLevel.Fatal:
                    Debug.LogError(message);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarning(message);
                    break;
            }
        }
    }
}

