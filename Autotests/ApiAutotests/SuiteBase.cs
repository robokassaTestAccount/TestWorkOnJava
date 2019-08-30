using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerHelperSpace;
using System;
using System.IO;

namespace ApiAutotests
{
    public class SuiteBase
    {
        public string TestName { get; set; }
        public TestContext TestContext { get; set; }
        public LoggerHelper Logger { get; set; }
        public AssertHelper AssertHelper { get; set; }
        public PostClient PostClient { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {     

            TestName = TestContext.TestName;           
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TestLogPath")))
            {
                Logger = new LoggerHelper(@"C:\TestResults", $"/{TestName}");
            }
            else
            {
                Logger = new LoggerHelper(Environment.GetEnvironmentVariable("TestLogPath"), $"/{TestName}");
            }
            AssertHelper = new AssertHelper(Logger);
            PostClient = PostClient.GetInstance(Logger);

            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TestLogPath")))
            {
                File.Delete(@"C:\TestResults" + $"/{TestName}.txt");
            }
            else
            {
                File.Delete(Environment.GetEnvironmentVariable("TestLogPath") + $"/{TestName}.txt");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TestLogPath")))
            {
                TestContext.AddResultFile(@"C:\TestResults" + $"/{TestName}.txt");
            }
            else
            {
                TestContext.AddResultFile(Environment.GetEnvironmentVariable("TestLogPath") + $"/{TestName}.txt");
            }
            AssertHelper.AssertRelease();
            ClearProcess();
        }
        private void ClearProcess()
        {
            string name = "chromedriver";//процесс, который нужно убить
            System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();//получим процессы
            foreach (System.Diagnostics.Process anti in etc)//обойдем каждый процесс
            {
                if (anti.ProcessName.ToLower().Contains(name.ToLower()))
                {
                    anti.Kill();//найдем нужный и убьем
                }
            }
        }
    }
}
