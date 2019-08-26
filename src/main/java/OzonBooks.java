
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
		watchAll = new SimpleWebElement("//a[contains(text(), '����� � ���������')]/..//span[contains(text(),"
										+ "'�������� ���')]","�������� ���");
		watchAll.Browser = Browser;
		return watchAll;
	}
	
	private SimpleWebElement history;
	public SimpleWebElement GetHistory()
	{
		history = null;
		history = new SimpleWebElement("//a[contains(text(), '�������')]","�������");
		history.Browser = Browser;
		return history;
	}
}
