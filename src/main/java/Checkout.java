
public class Checkout extends PageBase
{
	public Checkout(Browser browser) {
		super(browser);
		TargetSite = "https://www.ozon.ru/checkout";
	}

	private SimpleWebElement formOrder;
	public SimpleWebElement GetFormOrder()
	{
		formOrder = new SimpleWebElement("//button[@data-test-id='confirm-order-btn']","ќформить заказ");
		formOrder.Browser = Browser;
		return formOrder;
	}
}
