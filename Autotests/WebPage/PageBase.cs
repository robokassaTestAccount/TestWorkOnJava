using Core;
using LoggerHelperSpace;

namespace WebPage
{
    public abstract class PageBase
    {
        public Browser Browser { get; set; }
        public LoggerHelper Logger { get; set; }
        protected string Url { get; set; }
        public PageBase(Browser browser, LoggerHelper helper)
        {
            Browser = browser;
            Logger = helper;
        }

        public virtual void Open()
        {
            Url = TargetSite;

            Logger.WriteInfo($"Переход по адресу {Url}");
            Browser.Navigate(Url);
            Browser.WaitForAjaxComplete(4);
        }

        public string Title
        {

            get
            {
                return Browser.Title;
            }

        }

        public string TargetSite { get; set; }
    }
}
