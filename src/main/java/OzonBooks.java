
public class OzonBooks extends PageBase
{

	public OzonBooks(Browser browser) {
		super(browser);
		TargetSite = "https://www.ozon.ru/category/knigi-16500/";
	}
	
	private SimpleWebElement watchAll;
	public SimpleWebElement GetWatchAll()
	{
		watchAll = null;
		watchAll = new SimpleWebElement("//a[contains(text(), 'Детям и родителям')]/..//span[contains(text(),"
										+ "'Показать все')]","Показать все");
		watchAll.Browser = Browser;
		return watchAll;
	}
	
	private SimpleWebElement history;
	public SimpleWebElement GetHistory()
	{
		history = null;
		history = new SimpleWebElement("//a[contains(text(), 'История')]","История");
		history.Browser = Browser;
		return history;
	}
}
