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

            logger.Log(LogType.Error, "message");
            logger.Log(LogType.Log, "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogsAtSpecifiedLevelAndAbove()
        {
            logger.ConfigureLogging("*", LogType.Log);

            LogAssert.Expect(LogType.Log, "[*] message");
            LogAssert.Expect(LogType.Error, "[*] message");
            LogAssert.Expect(LogType.Warning, "[*] message");


            logger.Log(LogType.Log, "message");
            logger.Log(LogType.Error, "message");
            logger.Log(LogType.Warning, "message");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void CanCustomizeLevelByContext()
        {
            logger.ConfigureLogging("combat", LogType.Log);
            LogAssert.Expect(LogType.Error, "[*] message");
            LogAssert.Expect(LogType.Log, "[combat] message");

            logger.Log(LogType.Error, "message");
            logger.Log(LogType.Log, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogErrorIfLevelIsLog()
        {
            logger.ConfigureLogging("combat", LogType.Log);
            LogAssert.Expect(LogType.Error, "[combat] message");

            logger.Log(LogType.Error, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogErrorIfLevelIsWarning()
        {
            logger.ConfigureLogging("combat", LogType.Warning);
            LogAssert.Expect(LogType.Error, "[combat] message");

            logger.Log(LogType.Error, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogErrorIfLevelIsError()
        {
            logger.ConfigureLogging("combat", LogType.Error);
            LogAssert.Expect(LogType.Error, "[combat] message");

            logger.Log(LogType.Error, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void DoNotLogWarningIfLevelIsError()
        {
            logger.ConfigureLogging("combat", LogType.Error);

            logger.Log(LogType.Warning, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogWarningIfLevelIsWarning()
        {
            logger.ConfigureLogging("combat", LogType.Warning);
            LogAssert.Expect(LogType.Warning, "[combat] message");

            logger.Log(LogType.Warning, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }
        
        [Test]
        public void DoNotLogInfoIfLevelIsWarning()
        {
            logger.ConfigureLogging("combat", LogType.Warning);

            logger.Log(LogType.Log, "message", "combat");

            LogAssert.NoUnexpectedReceived();
        }
    }
}