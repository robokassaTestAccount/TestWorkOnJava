using Core;
using LoggerHelperSpace;
using WebElemenet;

namespace WebPage.accounts.google.com
{
    public class User : PageBase
    {
        public User(Browser browser, LoggerHelper Logger) : base(browser, Logger)
        {
            TargetSite = "https://accounts.google.com";
        }
        public WebElement Login
        {
            get
            {
                WebElement _local = new WebElement("//input[@name='identifier']", "identifier")
                {
                    Browser = Browser,
                    Log = Logger
                };
                return _local;
            }
        }

        public WebElement Next
        {
            get
            {
                WebElement _local = new WebElement("//div[@id='identifierNext']", "NExt")
                {
                    Browser = Browser,
                    Log = Logger
                };
                return _local;
            }
        }

        public WebElement Password
        {
            get
            {
                WebElement _local = new WebElement("//input[@name='password']", "password")
                {
                    Browser = Browser,
                    Log = Logger
                };
                return _local;
            }
        }

        public WebElement PasswordNext
        {
            get
            {
                WebElement _local = new WebElement("//div[@id='passwordNext']", "NExt")
                {
                    Browser = Browser,
                    Log = Logger
                };
                return _local;
            }
        }
    }
}
