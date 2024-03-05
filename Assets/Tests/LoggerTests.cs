using NUnit.Framework;
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
            LogAssert.Expect(LogType.Error, "[*] message");
            LogAssert.Expect(LogType.Log, "[*] message");

            logger.Log(LogLevel.Error, "message");
            logger.Log(LogLevel.Info, "message");

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
            logger.ConfigureLogging("*", LogLevel.Info);
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
        public void FatalMethodLogsAtErrorLevel()
        {
            LogAssert.Expect(LogType.Assert, "[*] message");
            LogAssert.Expect(LogType.Assert, "[*] message");

            logger.Fatal("message");
            logger.Fatal(() => "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ErrorMethodLogsAtErrorLevel()
        {
            LogAssert.Expect(LogType.Error, "[*] message");
            LogAssert.Expect(LogType.Error, "[*] message");

            logger.Error("message");
            logger.Error(() => "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void WarnMethodLogsAtWarningLevel()
        {
            LogAssert.Expect(LogType.Warning, "[*] message");
            LogAssert.Expect(LogType.Warning, "[*] message");

            logger.Warn("message");
            logger.Warn(() => "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void InfoMethodLogsAtLogLevel()
        {
            LogAssert.Expect(LogType.Log, "[*] message");
            LogAssert.Expect(LogType.Log, "[*] message");

            logger.Info("message");
            logger.Info(() => "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void DebugMethodLogsAtLogLevel()
        {
            logger.ConfigureLogging("*", LogLevel.Debug);
            LogAssert.Expect(LogType.Log, "[*] message");
            LogAssert.Expect(LogType.Log, "[*] message");

            logger.Debug("message");
            logger.Debug(() => "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void TraceMethodLogsAtLogLevel()
        {
            logger.ConfigureLogging("*", LogLevel.Trace);
            LogAssert.Expect(LogType.Log, "[*] message");
            LogAssert.Expect(LogType.Log, "[*] message");

            logger.Trace("message");
            logger.Trace(() => "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void MessageGeneratorNotCalledForUnloggedMessages()
        {
            LogAssert.Expect(LogType.Log, "[*] foobar");
            logger.Trace(() =>
            {
                Assert.Fail();
                return "";
            });
            logger.Info(() =>
            {
                return "foobar";
            });

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void CanUseUnityLogType()
        {
            LogAssert.Expect(LogType.Assert, "[*] foobar");
            logger.Log(LogType.Assert, "foobar");

            LogAssert.Expect(LogType.Error, "[*] foobar");
            logger.Log(LogType.Error, "foobar");

            LogAssert.Expect(LogType.Error, "[*] foobar");
            logger.Log(LogType.Exception, "foobar");

            LogAssert.Expect(LogType.Log, "[*] foobar");
            logger.Log(LogType.Log, "foobar");

            LogAssert.Expect(LogType.Warning, "[*] foobar");
            logger.Log(LogType.Warning, "foobar");

            LogAssert.NoUnexpectedReceived();
        }
    }
}