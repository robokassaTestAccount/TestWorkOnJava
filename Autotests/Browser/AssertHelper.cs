using LoggerHelperSpace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class AssertHelper
    {
        private List<string> _failedAsserts { get; set; }
        private LoggerHelper Logger { get; set; }
        public AssertHelper(LoggerHelper logger)
        {
            _failedAsserts = new List<string>();
            Logger = logger;
        }

        public void AssertIsTrue(bool Condition, string Message)
        {            
            Logger.WriteInfo(Message);
            Assert.IsTrue(Condition, $"Проверка: {Message} - не пройдена.");
        }

        public void AssertIsFalse(bool Condition, string Message)
        {
            Logger.WriteInfo(Message);
            Assert.IsFalse(Condition, $"Проверка: {Message} - не пройдена.");
        }

        public void AssertIsTrueAndAccumulate(bool Condition, string Message)
        {
            Logger.WriteInfo($"{Message} с аккумуляцией результата");
            if(!Condition)
            {
                _failedAsserts.Add(Message);
                Logger.WriteError($"Проверка: {Message} - не пройдена.");
            }
        }

        public void AssertIsFalseAndAccumulate(bool Condition, string Message)
        {
            Logger.WriteInfo($"{Message} с аккумуляцией результата");
            if (Condition)
            {
                _failedAsserts.Add(Message);
                Logger.WriteError($"Проверка: {Message} - не пройдена.");
            }
        }

        public void AssertIsFail(string Message)
        {
            Logger.WriteError(Message);
            Assert.Fail();
        }

        public void AssertRelease()
        {
            for(int i=0; i<_failedAsserts.Count; i++)
            {
                Logger.WriteError($"Проверка: {_failedAsserts.ElementAt(i)} не прошла");
                if(i+1 == _failedAsserts.Count)
                {
                    AssertIsFail($"Количество ошибок {_failedAsserts.Count}");
                }
            }
        }
    }
}
