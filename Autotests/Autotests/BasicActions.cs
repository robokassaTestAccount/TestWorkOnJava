namespace Autotests
{
    public class BasicActions : SuiteBase
    {
        public void Login()
        {
            Pages.User.Open();
            Pages.User.Login.SendKeys("futuracomesivan");
            Pages.User.Next.Click();
            Pages.User.Password.SendKeys("654321ytrewQ");
            Pages.User.PasswordNext.Click();
        }
    }
}
