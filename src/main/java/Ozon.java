import java.util.ArrayList;

public class Ozon extends PageBase
{

	public Ozon(Browser browser) 
	{
		super(browser);
		TargetSite = "https://www.ozon.ru/";
	}
	
	private SimpleWebElement allCatalogs;
	public SimpleWebElement GetAllCatalogs()
	{
		allCatalogs = null;
		allCatalogs = new SimpleWebElement("//div[@class='context-chip m-all-contexts']","��� �������");
		allCatalogs.Browser = Browser;
		return allCatalogs;
	}

	private SimpleWebElement books;
	public SimpleWebElement GetBooks()
	{
		books = null;
		books = new SimpleWebElement("//div[contains(text(),'�����')]","�����");
		books.Browser = Browser;
		return books;
	}
	
	private SimpleWebElement search;
	public SimpleWebElement GetSearch()
	{
		search = null;
		search = new SimpleWebElement("//button[@data-test-id='header-search-go']/..","������ � �����");
		search.Browser = Browser;
		return search;
	}
	
	private SimpleWebElement searchField;
	public SimpleWebElement GetSearchField()
	{
		searchField = null;
		searchField = new SimpleWebElement("//input[@data-test-id='header-search-input']","������ ������");
		searchField.Browser = Browser;
		return searchField;
	}
	
	
	private WebElementCollection goods;
	public ArrayList<SimpleWebElement> GetGoods()
	{
		goods = null;
		goods = new WebElementCollection("������", false);
		return goods.GetCollection("//button[@data-test-id='tile-buy-button']", Browser);
	}	
	
	
	private SimpleWebElement firstGood = null;
	public SimpleWebElement GetFirstGood()
	{		
		firstGood = null;
		firstGood = new SimpleWebElement("//span[contains(text(), '� �������')]/..","������ �����");
		firstGood.Browser = Browser;
		return firstGood;
	}	
	
	private SimpleWebElement counter;
	public SimpleWebElement GetCounter()
	{
		counter= null;
		counter = new SimpleWebElement("//span[@data-test-id='counter']","�������");
		counter.Browser = Browser;
		return counter;
	}	
}
