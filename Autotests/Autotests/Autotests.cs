using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Autotests
{
    [TestClass]
    public class Autotests : BasicActions
    {
        [TestMethod]
        public void GmailLogin()
        {
            Login();
            string welcomeText = Pages.MyAccount.YouAreWelcome.Text;
            AssertHelper.AssertIsTrueAndAccumulate(welcomeText == "Добро пожаловать, " +
                                                   "Тестовый ДляТестов!",
                                                   $"Логин успешен, " +
                                                   $"текст сообщения {welcomeText}");
        }
    }
}
