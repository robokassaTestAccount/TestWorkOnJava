using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace LoggerHelperSpace
{
    public class LoggerHelper
    {
        public LoggerHelper(string filePath, string name)
        {
            LoggingConfiguration Config = new LoggingConfiguration();
            FileTarget fileTarget = new FileTarget("fileTarget")
            {
                FileName = $"{filePath}/{name}.txt",
                Layout = "${longdate} ${level} ${message}  ${exception}"
            };
            Config.AddTarget(fileTarget);
            Config.AddRuleForAllLevels(fileTarget);
            LogManager.Configuration = Config;

            Log = LogManager.GetLogger("logger");
            
        }

        private Logger Log { get; set; }

        public void WriteInfo(string Info)
        {
            Log.Info(Info);
        }

        public void WriteError(string Error, Exception exception)
        {
            Log.Error(exception, Error);
        }
        public void WriteError(string Error)
        {
            Log.Error(Error);
        }
    }

}
