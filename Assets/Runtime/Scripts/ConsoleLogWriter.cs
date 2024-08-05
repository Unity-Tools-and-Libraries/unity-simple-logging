using System;
using System.Collections.Generic;
using UnityEngine;

namespace io.github.thisisnozaku.logging {
    /**
     * 
     */
    public class ConsoleLogConsumer : ILogConsumer
    {
        public static ConsoleLogConsumer CONSUMER = new ConsoleLogConsumer();
        private HashSet<string> deduplicationSet = new HashSet<string>();

        public void Flush()
        {
            deduplicationSet.Clear();
        }

        public void Log(LogLevel level, string message)
        {
            if(!deduplicationSet.Add(message))
            {
                return;
            }
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

