using System;
using UnityEngine;
namespace io.github.thisisnozaku.logging
{
	public interface ILogConsumer
	{
		void Log(LogLevel level, string message);

		void Flush();
	}
}
