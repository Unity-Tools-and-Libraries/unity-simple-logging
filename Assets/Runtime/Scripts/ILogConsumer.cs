using System;
using UnityEngine;
namespace io.github.thisisnozaku.logging
{
	public interface ILogConsumer
	{
		void Log(LogType level, string message);
	}
}
