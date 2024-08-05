using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace io.github.thisisnozaku.logging.tests
{
    public class LoggerTest
    {
        LoggingModule logger;

        [SetUp]
        public void setup()
        {
            logger = new LoggingModule();
        }

        [Test]
        public void LogsAllLevelsByDefault()
        {
            LogAssert.Expect(LogType.Log, "[*] message");
            LogAssert.Expect(LogType.Error, "[*] message");

            logger.Log(LogLevel.Info, "message");
            logger.Log(LogLevel.Error, "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogsAtSpecifiedLevelAndAbove()
        {
            logger.ConfigureLogging("*", LogLevel.Info);

            LogAssert.Expect(LogType.Log, "[*] message");
            LogAssert.Expect(LogType.Error, "[*] message");
            LogAssert.Expect(LogType.Warning, "[*] message");


            logger.Log(LogLevel.Info, "message");
            logger.Log(LogLevel.Error, "message");
            logger.Log(LogLevel.Warn, "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void CanCustomizeLevelByContext()
        {
            logger.ConfigureLogging("*", LogLevel.Trace);
            LogAssert.Expect(LogType.Error, "[*] message");
            LogAssert.Expect(LogType.Log, "[combat] message");

            logger.Log(LogLevel.Error, "message");
            logger.Log(LogLevel.Info, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogErrorIfLevelIsLog()
        {
            logger.ConfigureLogging("combat", LogLevel.Info);
            LogAssert.Expect(LogType.Error, "[combat] message");

            logger.Log(LogLevel.Error, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogErrorIfLevelIsWarning()
        {
            logger.ConfigureLogging("combat", LogLevel.Warn);
            LogAssert.Expect(LogType.Error, "[combat] message");

            logger.Log(LogLevel.Error, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogErrorIfLevelIsError()
        {
            logger.ConfigureLogging("combat", LogLevel.Error);
            LogAssert.Expect(LogType.Error, "[combat] message");

            logger.Log(LogLevel.Error, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void DoNotLogWarningIfLevelIsError()
        {
            logger.ConfigureLogging("combat", LogLevel.Error);

            logger.Log(LogLevel.Warn, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogWarningIfLevelIsWarning()
        {
            logger.ConfigureLogging("combat", LogLevel.Warn);
            LogAssert.Expect(LogType.Warning, "[combat] message");

            logger.Log(LogLevel.Warn, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void DoNotLogInfoIfLevelIsWarning()
        {
            logger.ConfigureLogging("combat", LogLevel.Warn);

            logger.Log(LogLevel.Info, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogCallsToLogSink()
        {
            var logSink = new TestSink();
            logger.ConfigureLogging("combat", LogLevel.Info, logSink);

            logger.Log(LogLevel.Info, "", "combat");

            Assert.IsTrue(logSink.called);
        }

        [Test]
        public void LogToMultipleSink()
        {
            var sinks = new TestSink[] { new TestSink(), new TestSink() };
            logger.ConfigureLogging("combat", LogLevel.Info, sinks);

            logger.Log(LogLevel.Info, "", "combat");

            Assert.IsTrue(sinks.All(s => s.called));
        }

        [Test]
        public void MultipleContextsAppearInMessage()
        {
            logger.ConfigureLogging("foo", LogLevel.Info);
            logger.ConfigureLogging("bar", LogLevel.Info);

            LogAssert.Expect(LogType.Log, "[foo][bar] message");

            logger.Log(LogLevel.Info, "message", "foo", "bar");

            LogAssert.NoUnexpectedReceived();
        }

        public class TestSink : ILogConsumer
        {
            public bool called { get; private set; }

            public void Flush()
            {

            }

            public void Log(LogLevel level, string message)
            {
                called = true;
            }
        }
    }
}