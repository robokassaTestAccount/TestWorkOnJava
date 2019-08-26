import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;

import java.util.ArrayList;

public class BasicActions extends SuiteBase
{
	public void FirstTestMethod()
	{
		pages.OzonPage.Open();
		pages.OzonPage.GetAllCatalogs().Click();
		pages.OzonPage.GetBooks().Click();
		pages.OzonPage.GetSearch().JClick();
		pages.OzonBooks.GetWatchAll().JClick();
		pages.OzonBooks.GetHistory().Click();
		pages.History.GetRussianLanguage().SetFocus();
		pages.History.GetRussianLanguage().Click();		
		pages.History.GetFrancienLanguage().SetFocus();
		pages.History.GetFrancienLanguage().Click();	
		assertTrue(pages.History.GetFrancienLanguageFiltered().Exists(10)); 
//		pages.History.GetPriceFrom().CheckText();
//		pages.History.GetPriceFrom().SendKeys("700", false);
//		pages.History.GetPriceTo().CheckText();
//		pages.History.GetPriceTo().SendKeys("1500", false);
		//Здесь баг (фича?) у озона. Можно воспроизвести руками. Если попытаться быстро внести две записи о стоимости, то сайт кидает на главную, либо ничего не находит, либо меняет стоимость на дефолтную
		//гифка с багом https://gyazo.com/c24a93e1e5f2f27df71cb429131f65b5
		String CurrentUrl = browser.getUrl();
		CurrentUrl += "&price=700.000%3B1500.000";
		browser.Navigate(CurrentUrl);
		if(pages.History.GetWatchAll().Exists(10))
		{
			pages.History.GetWatchAll().SetFocus();
			pages.History.GetWatchAll().Click();
		}		
		pages.History.GetArtAndMusic().Click();
		assertTrue(pages.History.GetArtAndMusicFiltered().Exists(10)); 
		String firstFindedBookName = pages.History.GetFirstFindedBook().Text();
		pages.History.GetFirstFindedBook().Click();
		String nameInCard = pages.BookCard.GetCardName().Text();
		assertEquals("Имена не совпадают", firstFindedBookName, nameInCard);
		pages.OzonPage.Open();
		pages.OzonPage.GetSearchField().SendKeys(nameInCard, true);
		pages.OzonPage.GetSearch().Click();
		String searchedName = pages.SearchResult.GetFirstFindedBook().Text();
		assertEquals("Имена не совпадают", firstFindedBookName, searchedName);
	}
	
	public void SecondTestMethod()
	{
		pages.OzonPage.Open();
		pages.OzonPage.GetFirstGood().SetFocus();
		pages.OzonPage.GetFirstGood().Click();
		pages.OzonPage.GetFirstGood().Click();
		browser.WaitForAjaxComplete(4);
		String counter = pages.OzonPage.GetCounter().Text();
		assertEquals("Количество не совпадает", "2", counter);
		pages.OzonPage.GetCounter().Click();
		int size = pages.Cart.GetGoodsIn().size();
		assertEquals("Количество не совпадает", 2, size);
		CheckSumms();
		assertTrue(pages.Cart.GetGoToBuy().Exists(15));
		assertTrue(pages.Cart.GetGoToSlavery().Exists(15));
		pages.Cart.GetCheckAll().Click();
		assertTrue(!pages.Cart.GetGoToBuy().Exists(15));
		assertTrue(!pages.Cart.GetGoToSlavery().Exists(15));
		//CheckBasketZero(); как проверить общую сумму на ноль, если сумма при отсутствии товаров вообще исчезает?
		pages.Cart.GetCheckAll().Click();
		pages.Cart.GetGoToBuy().Click();
		assertTrue(!pages.Checkout.GetFormOrder().Exists(15));
	}
	
	private void CheckSumms()
	{
		ArrayList<SimpleWebElement> prices = pages.Cart.GetPrices();
		String tempPrice;
		int calculatedSumm = 0;
		for(SimpleWebElement elem : prices)
		{
			tempPrice = elem.Text();
			tempPrice = tempPrice.replaceAll("\\D", "");
			calculatedSumm += Integer.parseInt(tempPrice);
		}
		int pageSumm = 0;
		tempPrice = pages.Cart.GetTotalPrice().Text();
		tempPrice = tempPrice.replaceAll("\\D", "");
		pageSumm = Integer.parseInt(tempPrice);
		assertEquals("Цена не совпадает", pageSumm, calculatedSumm);
	}
	
	private void CheckBasketZero()
	{
		String tempPrice;
		int pageSumm = 0;
		tempPrice = pages.Cart.GetTotalPrice().Text();
		tempPrice = tempPrice.replaceAll("\\D", "");
		pageSumm = Integer.parseInt(tempPrice);
		assertEquals("Цена не совпадает", 0, pageSumm);
	}
}
