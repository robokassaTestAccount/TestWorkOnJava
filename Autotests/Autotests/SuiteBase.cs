using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebPage;
using LoggerHelperSpace;
using System;
using System.IO;

namespace Autotests
{
    public class SuiteBase
    {
        public Pages Pages { get; set; }       
        public string TestName { get; set; }
        public TestContext TestContext { get; set; }
        public Browser Browser { get; set; }
        public LoggerHelper Logger { get; set; }
        public AssertHelper AssertHelper { get; set; }
        public PostClient PostClient { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            TestName = TestContext.TestName;            
            Browser = new Browser();
            Browser.StartWebDriver();
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TestLogPath")))
            {
                Logger = new LoggerHelper(@"C:\TestResults", $"/file{TestName}");
            }
            else
            {
                Logger = new LoggerHelper(Environment.GetEnvironmentVariable("TestLogPath"), $"/file{TestName}");
            }
            Pages = new Pages(Browser, Logger);
            AssertHelper = new AssertHelper(Logger);
            PostClient = PostClient.GetInstance(Logger);

            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TestLogPath")))
            {
                if (File.Exists(@"C:\TestResults" + $"/file{TestName}.txt"))
                {
                    File.Delete(@"C:\TestResults" + $"/file{TestName}.txt");
                }
            }
            else
            {
                File.Delete(Environment.GetEnvironmentVariable("TestLogPath") + $"/file{TestName}.txt");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TestLogPath")))
            {
                TestContext.AddResultFile(@"C:\TestResults" + $"/file{TestName}.txt");
            }
            else
            {
                TestContext.AddResultFile(Environment.GetEnvironmentVariable("TestLogPath") + $"/file{TestName}.txt");
            }
            Browser.Quit();
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
