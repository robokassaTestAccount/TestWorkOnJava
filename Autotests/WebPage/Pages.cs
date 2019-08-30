using Core;
using LoggerHelperSpace;

namespace WebPage
{
    public class Pages
    {
        public accounts.google.com.User User { get; set; }
        public accounts.google.com.MyAccount MyAccount { get; set; }
        public Pages(Browser browser, LoggerHelper logger)
        {            
            User = new accounts.google.com.User(browser, logger);
            MyAccount = new accounts.google.com.MyAccount(browser, logger);
        }
    }
}
