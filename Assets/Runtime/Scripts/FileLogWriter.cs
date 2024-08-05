using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace io.github.thisisnozaku.logging
{
    public class FileLogWriter : ILogConsumer
    {
        public readonly string Destination;
        private HashSet<string> deduplicationSet = new HashSet<string>();

        public FileLogWriter(string destination)
        {
            this.Destination = destination;
        }

        public void Flush()
        {

        }

        public void Log(LogLevel level, string message)
        {
            if (!deduplicationSet.Add(message))
            {
                return;
            }
            StreamWriter outputFile = null;
            try
            {
                outputFile = File.AppendText(Destination);
                outputFile.Write(message + Environment.NewLine);
                outputFile.Flush();
            } catch (IOException ex)
            {
                Debug.LogException(ex);
            } finally
            {
                if (outputFile != null)
                {
                    outputFile.Close();
                }
            }
        }
    }
}
