using io.github.thisisnozaku.logging;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
namespace io.github.thisisnozaku.logging.tests
{
    public class HierarchicalTests
    {
        LoggingModule logger;

        [SetUp]
        public void setup()
        {
            logger = new LoggingModule();
        }

        [Test]
        public void CanConfigureHierarchicalContexts()
        {
            logger.ConfigureLogging("hello.world", LogLevel.Trace);
            LogAssert.Expect(LogType.Log, "[hello.world] message");

            logger.Log(LogLevel.Trace, "message", "hello.world");
        }

        [Test]
        public void CanConfigureChildContextsAtDifferentLevels()
        {
            logger.ConfigureLogging("hello", LogLevel.Trace);
            logger.ConfigureLogging("hello.world", LogLevel.Error);

            LogAssert.Expect(LogType.Error, "[hello] message");
            logger.Log(LogLevel.Trace, "message", "hello");
            logger.Log(LogLevel.Error, "message", "hello");

            LogAssert.Expect(LogType.Error, "[hello.world] message");
            logger.Log(LogLevel.Error, "message", "hello.world");
        }

        [Test]
        public void CanConfigureChildContextsImplicitly()
        {
            // Doesn't log
            logger.Log(LogLevel.Trace, "message", "hello");

            logger.ConfigureLogging("hello.*", LogLevel.Trace);
            // Doesn't log
            logger.Log(LogLevel.Trace, "message", "hello");

            LogAssert.Expect(LogType.Log, "[hello.world] message");
            // Does log
            logger.Log(LogLevel.Trace, "message", "hello.world");
        }
    }
}