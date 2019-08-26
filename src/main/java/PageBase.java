
public abstract class PageBase 
{
	 public Browser Browser;
     protected String Url;
     public PageBase(Browser browser)
     {
         Browser = browser;
     }

     public void Open()
     {
         Url = TargetSite;
         Browser.Navigate(Url);
         Browser.WaitForAjaxComplete(4);
     }

     public String TargetSite;
}
