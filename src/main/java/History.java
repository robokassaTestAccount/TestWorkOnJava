
public class History extends PageBase
{
	public History(Browser browser) {
		super(browser);
		TargetSite = "https://www.ozon.ru/category/istoriya-40037/";
	}
	
	private SimpleWebElement russianLanguage;
	public SimpleWebElement GetRussianLanguage()
	{
		russianLanguage = new SimpleWebElement("//*[contains(text(),'Русский')]/../span[@class='checkmark']","Русский");
		russianLanguage.Browser = Browser;
		return russianLanguage;
	}	
	
	private SimpleWebElement francienLanguage;
	public SimpleWebElement GetFrancienLanguage()
	{
		francienLanguage = new SimpleWebElement("//*[contains(text(),'Французский')]/../span[@class='checkmark']","Французский");
		francienLanguage.Browser = Browser;
		return francienLanguage;
	}
	
	private SimpleWebElement francienLanguageFiltered;
	public SimpleWebElement GetFrancienLanguageFiltered()
	{
		francienLanguageFiltered = new SimpleWebElement("//div[contains(text(), 'Очистить')]/..//span[contains(text(), 'Фран')]","Французский в фильтрах сверху");
		francienLanguageFiltered.Browser = Browser;
		return francienLanguageFiltered;
	}
	
	private SimpleWebElement priceFrom;
	public SimpleWebElement GetPriceFrom()
	{
		priceFrom = new SimpleWebElement("//input[@data-test-id='range-filter-from-input']","Цена от");
		priceFrom.Browser = Browser;
		return priceFrom;
	}
	
	private SimpleWebElement priceTo;
	public SimpleWebElement GetPriceTo()
	{
		priceTo = new SimpleWebElement("//input[@data-test-id='range-filter-to-input']","Цена до");
		priceTo.Browser = Browser;
		return priceTo;
	}
	
	private SimpleWebElement watchAll;
	public SimpleWebElement GetWatchAll()
	{
		watchAll = new SimpleWebElement("//span[@data-test-id='filter-block-childrenbookgenre-show-all']","Показать все");
		watchAll.Browser = Browser;
		return watchAll;
	}
	
	private SimpleWebElement artAndMusic;
	public SimpleWebElement GetArtAndMusic()
	{
		artAndMusic = new SimpleWebElement("//*[contains(text(),'Искусс')]/../span[@class='checkmark']","Искусство");
		artAndMusic.Browser = Browser;
		return artAndMusic;
	}
	
	private SimpleWebElement artAndMusicFiltered;
	public SimpleWebElement GetArtAndMusicFiltered()
	{
		artAndMusicFiltered = new SimpleWebElement("//div[contains(text(), 'Очистить')]/..//span[contains(text(), 'Иску')]","Искусство выбранный фильтр");
		artAndMusicFiltered.Browser = Browser;
		return artAndMusicFiltered;
	}
	
	private SimpleWebElement firstFindedBook;
	public SimpleWebElement GetFirstFindedBook()
	{
		firstFindedBook = new SimpleWebElement("//div[@data-index='0']//a[@data-test-id='tile-name']","Первая найденая книга");
		firstFindedBook.Browser = Browser;
		return firstFindedBook;
	}
	
	
}
