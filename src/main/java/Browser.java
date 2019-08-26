import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.concurrent.TimeUnit;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Point;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.FluentWait;
import org.openqa.selenium.support.ui.Wait;
import org.openqa.selenium.support.ui.WebDriverWait;

public class Browser 
{
	private String _mainWindowHandler;
	private WebDriver Driver;
	private String Type;
	public Browser() 
	{
		Type = "Chrome";
	}
	
	public Browser(String type)
	{
		Type = type;
	}

	public void StartWebDriver() 
	{
		switch(Type)
		{
			case "Chrome":
			{
				Driver = StartChrome();
				break;
			}
			case "Firefox":
			{
				Driver = StartFirefox();
				break;
			}
		}
		Driver.manage().deleteAllCookies();
		Driver.manage().timeouts().setScriptTimeout(60, TimeUnit.SECONDS);
		Driver.manage().timeouts().pageLoadTimeout(60, TimeUnit.SECONDS);
		Driver.manage().window().maximize();
		_mainWindowHandler = Driver.getWindowHandle();
	}

	private FirefoxDriver StartFirefox()
	{
		Path currentRelativePath = Paths.get("");
		String s = currentRelativePath.toAbsolutePath().toString();
		
		System.setProperty("webdriver.gecko.driver",s+"\\geckodriver.exe");
		return new FirefoxDriver();
	}
	
	private ChromeDriver StartChrome() 
	{
		Path currentRelativePath = Paths.get("");
		String s = currentRelativePath.toAbsolutePath().toString();

		System.setProperty("webdriver.chrome.driver",s+"\\chromedriver.exe");
		ChromeOptions chromeOptions = new ChromeOptions();
		chromeOptions.addArguments("download.default_directory", "C:\\TestData");
		chromeOptions.addArguments("safebrowsing.enabled", "false");
		chromeOptions.addArguments("acceptInsecureCerts", "true");
		chromeOptions.addArguments("disable-extensions");
		chromeOptions.addArguments("ignore-certificate-errors", "ignore-urlfetcher-cert-requests",
				"allow-insecure-localhost");
		return new ChromeDriver(chromeOptions);
	}

	public WebElement FindElement(String XPath) 
	{
		WaitForAjaxComplete(4);
		WebElement element;

		Wait<WebDriver> wait = new FluentWait<WebDriver>(Driver)
				.withTimeout(60, TimeUnit.SECONDS)
				.pollingEvery(500, TimeUnit.MILLISECONDS)
				.ignoring(Exception.class);
		try 
		{
			element = wait.until(ExpectedConditions.elementToBeClickable(By.xpath(XPath)));
		} 
		catch (Exception e) 
		{
			//здесь был логгер, но его нет желани€ доделывать (так же везде должно быть)
			return null;
		}

		return element;
	}

	public ArrayList<WebElement> FindElements(String XPath) 
	{
		WaitForAjaxComplete(15);
		ArrayList<WebElement> elements = new ArrayList<WebElement>();

		Wait<WebDriver> wait = new FluentWait<WebDriver>(Driver)
				.withTimeout(60, TimeUnit.SECONDS)
				.pollingEvery(1, TimeUnit.MILLISECONDS)
				.ignoring(Exception.class);

		try 
		{
			List<WebElement> finded = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(XPath)));
			for (WebElement elem : finded) 
			{
				elements.add(elem);
			}
		} 
		catch (Exception e)
		{
			//здесь был логгер, но его нет желани€ доделывать (так же везде должно быть)
			return null;
		}

		return elements;
	}

	public WebElement FindHiddenElement(String XPath, int timeOut) 
	{
		WaitForAjaxComplete(5);
		WebElement element;

		Wait<WebDriver> wait = new FluentWait<WebDriver>(Driver)
				.withTimeout(60, TimeUnit.SECONDS)
				.pollingEvery(1, TimeUnit.MILLISECONDS)
				.ignoring(Exception.class);
		try 
		{
			element = wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(XPath)));
		} 
		catch (Exception e) 
		{
			//здесь был логгер, но его нет желани€ доделывать (так же везде должно быть)
			return null;
		}

		return element;
	}

	public void Navigate(String Url) 
	{
		Driver.navigate().to(Url);
	}

	public void SwitchToMain()
	{
		Driver.switchTo().window(_mainWindowHandler);
	}

	public void SwitchToNewWindow()
	{
		Set<String> handles = Driver.getWindowHandles();
		List<String> list = new ArrayList<String>(handles);
		String handle = list.get(list.size() - 1);
		Driver.switchTo().window(handle);
	}

	public Set<String> GetHandlers()
	{
		return Driver.getWindowHandles();
	}

	public void SwitchToNewWindow(String Handl)
	{
		Driver.switchTo().window(Handl);
	}

	public void CloseActiveWindows() 
	{
		Set<String> handles = Driver.getWindowHandles();
		for (String handl : handles) 
		{
			handles = Driver.getWindowHandles();
			SwitchToNewWindow(handl);
			if (handl != _mainWindowHandler) 
			{
				Driver.close();
			}
		}
	}

	public void OpenWindow() 
	{
		ExecuteJavaScript("window.open('your url','_blank');", null);
	}

	public void Refresh() 
	{
		Driver.navigate().refresh();
	}

	public String getUrl() 
	{
		WaitForAjaxComplete(3);
		return Driver.getCurrentUrl();
	}

	public void Quit() 
	{
		Driver.quit();
	}

	public void AllertAccept() 
	{
		WebDriverWait wait = new WebDriverWait(Driver, 30 /* timeout in seconds */);
		if (wait.until(ExpectedConditions.alertIsPresent()) != null) 
		{
			Driver.switchTo().alert().accept();
		}
	}

	public Object ExecuteJavaScript(String javaScript, Object element) 
	{
		JavascriptExecutor javaScriptExecutor = (JavascriptExecutor) Driver;
		try
		{
			return javaScriptExecutor.executeScript(javaScript, element);
		} 
		catch (Exception e)
		{
			return javaScriptExecutor.executeScript(javaScript, element);
		}
	}

	public void WaitForAjaxComplete(int timeoutInSeconds)
	{
		//метод изменен. —тарый в €ве не заработал.
		String firstSource = Driver.getPageSource();
		try
		{
			Thread.sleep(100);
		}
		catch(Exception e)
		{
			//nothing to do now
		}
		String secondSource = Driver.getPageSource();
		int allTime = 0;
		while(firstSource != secondSource)
		{
			if(allTime >= timeoutInSeconds)
			{
				break;
			}
			firstSource = Driver.getPageSource();
			try
			{
				Thread.sleep(100);
			}
			catch(Exception ex)
			{
				//nothing to do now
			}
			secondSource = Driver.getPageSource();
			allTime+=100;
		}
	}	

	public void ScrollElementToCenter(Point location)
	{
		int height = GetBrowserWindowInnerHeight() / 2;
		location.y = location.y - height;
		ScrollToElement(location);
	}

	public void ScrollToElement(Point location) 
	{
		ExecuteJavaScript("window.scrollTo("+location.x+","+ location.y+");", null);
	}

	public int GetBrowserWindowInnerHeight() 
	{
		return  Integer.parseInt(ExecuteJavaScript("return window.innerHeight;", null).toString());
	}
}