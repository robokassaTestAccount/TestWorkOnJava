import java.util.ArrayList;

public class Cart extends PageBase
{

	public Cart(Browser browser) {
		super(browser);
		TargetSite = "https://www.ozon.ru/cart";
	}
	
	private WebElementCollection goodsIn;
	public ArrayList<SimpleWebElement> GetGoodsIn()
	{
		goodsIn = new WebElementCollection("������ � �������", false);
		return goodsIn.GetCollection("//div[@data-test-id='cart-split-item']", Browser);
	}
	
	private WebElementCollection prices;
	public ArrayList<SimpleWebElement> GetPrices()
	{
		prices = new WebElementCollection("���� �� ������", false);
		return prices.GetCollection("//div[@class='cart-item__column m-price']/div[1]//span", Browser);
	}
	
	private SimpleWebElement totalPrice;
	public SimpleWebElement GetTotalPrice()
	{
		totalPrice = new SimpleWebElement("//span[@class='total-middle-footer-text']","����� ����");
		totalPrice.Browser = Browser;
		return totalPrice;
	}

	private SimpleWebElement goToBuy;
	public SimpleWebElement GetGoToBuy()
	{
		goToBuy = new SimpleWebElement("//button[@data-test-id='cart-proceed-to-checkout-btn']","������� � ����������");
		goToBuy.Browser = Browser;
		return goToBuy;
	}
	
	private SimpleWebElement goToSlavery;
	public SimpleWebElement GetGoToSlavery()
	{
		goToSlavery = new SimpleWebElement("//span[contains(text(), '����')]/..","������ ��������� �� ������� (������)");
		goToSlavery.Browser = Browser;
		return goToSlavery;
	}
	
	private SimpleWebElement checkAll;
	public SimpleWebElement GetCheckAll()
	{
		checkAll = new SimpleWebElement("//label[@data-test-id='cart-select-all-btn']/span","������� ��� �����");
		checkAll.Browser = Browser;
		return checkAll;
	}
	
	
}
