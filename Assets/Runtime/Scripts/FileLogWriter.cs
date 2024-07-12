using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace io.github.thisisnozaku.logging
{
    public class FileLogWriter : ILogConsumer
    {
        public readonly string Destination;

        public FileLogWriter(string destination)
        {
            this.Destination = destination;
        }

        public void Log(LogType level, string message)
        {
            try
            {

            } catch (IOException ex)
            {

            }
        }
    }
}