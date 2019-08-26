
public class Pages 
{
	public Ozon OzonPage;
	public OzonBooks OzonBooks;
	public History History;
	public BookCard BookCard;
	public SearchResult SearchResult;
	public Cart Cart;
	public Checkout Checkout;
    public Pages(Browser browser)
    {
    	Cart = new Cart(browser);
    	OzonPage = new Ozon(browser);
    	OzonBooks = new OzonBooks(browser);
    	History = new History(browser);
    	BookCard = new BookCard(browser);
    	SearchResult = new SearchResult(browser);
    	Checkout = new Checkout(browser);
    }
}
