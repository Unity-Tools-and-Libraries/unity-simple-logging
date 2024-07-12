using System;
using UnityEngine;

namespace io.github.thisisnozaku.logging {
    /**
     * 
     */
    public class ConsoleLogConsumer : ILogConsumer
    {
        public static ConsoleLogConsumer CONSUMER = new ConsoleLogConsumer();
        public void Log(LogType level, string message)
        {
            switch(level){
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
            }
        }
    }
}

