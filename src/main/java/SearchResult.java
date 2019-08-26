
public class SearchResult extends PageBase
{

	public SearchResult(Browser browser) {
		super(browser);
		TargetSite = "";
	}

	private SimpleWebElement firstFindedBook;
	public SimpleWebElement GetFirstFindedBook()
	{
		firstFindedBook = new SimpleWebElement("//div[@data-index='0']//a[@data-test-id='tile-name']","Первая найденая книга");
		firstFindedBook.Browser = Browser;
		return firstFindedBook;
	}
}
