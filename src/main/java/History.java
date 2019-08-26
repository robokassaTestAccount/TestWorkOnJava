
public class History extends PageBase
{
	public History(Browser browser) {
		super(browser);
		TargetSite = "https://www.ozon.ru/category/istoriya-40037/";
	}
	
	private SimpleWebElement russianLanguage;
	public SimpleWebElement GetRussianLanguage()
	{
		russianLanguage = new SimpleWebElement("//*[contains(text(),'�������')]/../span[@class='checkmark']","�������");
		russianLanguage.Browser = Browser;
		return russianLanguage;
	}	
	
	private SimpleWebElement francienLanguage;
	public SimpleWebElement GetFrancienLanguage()
	{
		francienLanguage = new SimpleWebElement("//*[contains(text(),'�����������')]/../span[@class='checkmark']","�����������");
		francienLanguage.Browser = Browser;
		return francienLanguage;
	}
	
	private SimpleWebElement francienLanguageFiltered;
	public SimpleWebElement GetFrancienLanguageFiltered()
	{
		francienLanguageFiltered = new SimpleWebElement("//div[contains(text(), '��������')]/..//span[contains(text(), '����')]","����������� � �������� ������");
		francienLanguageFiltered.Browser = Browser;
		return francienLanguageFiltered;
	}
	
	private SimpleWebElement priceFrom;
	public SimpleWebElement GetPriceFrom()
	{
		priceFrom = new SimpleWebElement("//input[@data-test-id='range-filter-from-input']","���� ��");
		priceFrom.Browser = Browser;
		return priceFrom;
	}
	
	private SimpleWebElement priceTo;
	public SimpleWebElement GetPriceTo()
	{
		priceTo = new SimpleWebElement("//input[@data-test-id='range-filter-to-input']","���� ��");
		priceTo.Browser = Browser;
		return priceTo;
	}
	
	private SimpleWebElement watchAll;
	public SimpleWebElement GetWatchAll()
	{
		watchAll = new SimpleWebElement("//span[@data-test-id='filter-block-childrenbookgenre-show-all']","�������� ���");
		watchAll.Browser = Browser;
		return watchAll;
	}
	
	private SimpleWebElement artAndMusic;
	public SimpleWebElement GetArtAndMusic()
	{
		artAndMusic = new SimpleWebElement("//*[contains(text(),'������')]/../span[@class='checkmark']","���������");
		artAndMusic.Browser = Browser;
		return artAndMusic;
	}
	
	private SimpleWebElement artAndMusicFiltered;
	public SimpleWebElement GetArtAndMusicFiltered()
	{
		artAndMusicFiltered = new SimpleWebElement("//div[contains(text(), '��������')]/..//span[contains(text(), '����')]","��������� ��������� ������");
		artAndMusicFiltered.Browser = Browser;
		return artAndMusicFiltered;
	}
	
	private SimpleWebElement firstFindedBook;
	public SimpleWebElement GetFirstFindedBook()
	{
		firstFindedBook = new SimpleWebElement("//div[@data-index='0']//a[@data-test-id='tile-name']","������ �������� �����");
		firstFindedBook.Browser = Browser;
		return firstFindedBook;
	}
	
	
}
