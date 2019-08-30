using Core;
using LoggerHelperSpace;
using WebElemenet;

namespace WebPage.accounts.google.com
{
    public class MyAccount : PageBase
    {
        public MyAccount(Browser browser, LoggerHelper Logger) : base(browser, Logger)
        {
            TargetSite = "https://accounts.google.com";
        }

        public WebElement YouAreWelcome
        {
            get
            {
                WebElement _local = new WebElement("//h1", "Welcome Text")
                {
                    Browser = Browser,
                    Log = Logger
                };
                return _local;
            }
        }
    }
}
