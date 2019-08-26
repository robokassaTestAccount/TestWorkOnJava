
public class BookCard extends PageBase
{

	public BookCard(Browser browser)
	{
		super(browser);		
		TargetSite="";
	}

	private SimpleWebElement cardName;
	public SimpleWebElement GetCardName()
	{
		cardName = new SimpleWebElement("//h1[@itemprop='name']","Название");
		cardName.Browser = Browser;
		return cardName;
	}
}
