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

        public void Log(LogLevel level, string message)
        {
            StreamWriter outputFile = null;
            try
            {
                outputFile = File.AppendText(Destination);
                outputFile.Write(message);
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