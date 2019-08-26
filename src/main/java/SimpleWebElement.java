import org.openqa.selenium.Keys;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.Select;

public class SimpleWebElement 
{
	public Browser Browser;
	public Boolean OnlyInDom;
	public String XPath;
	private WebElement Element;
	public String Description;
	private Boolean AlreadyFinded;

	public SimpleWebElement(String XPath, String Description) 
	{
		this.XPath = XPath;
		this.Description = Description;
		AlreadyFinded = false;
		OnlyInDom = false;
	}

	public void Click() 
	{
		Element = CheckElementConditions(60);
		try
		{
			Element.click();
		} 
		catch (Exception ex)
		{
			// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
		}
	}

	public Boolean Exists(int timeOut) 
	{
		try 
		{
			OnlyInDom = true;
			Element = CheckElementConditions(timeOut);
			if (Element == null)
			{
				return false;
			}
			if (Element.isDisplayed() && Element.isEnabled())
			{
				OnlyInDom = false;
				return true;
			} 
			else 
			{
				OnlyInDom = false;
				return false;
			}
		}
		catch (Exception ex) 
		{
			// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
			return false;
		}
	}

	public SimpleWebElement ConvertToSimpleWebElement(WebElement elem) 
	{
		Element = elem;
		AlreadyFinded = true;
		return this;
	}

	enum SelectTypes 
	{
		ByValue, ByText
	}

	private void SelectCommon(String option, SelectTypes selectType)
    {
            Element = CheckElementConditions(60);

            switch (selectType)
            {
                case ByValue:
                    new Select(Element).selectByValue(option);
                    return;
                case ByText:
                    new Select(Element).selectByVisibleText(option);
                    return;
            }
        }

	public void SelectByText(String optionText)
	{
		SelectCommon(optionText, SelectTypes.ByText);
	}

	public void CheckText()
	{
		Element = CheckElementConditions(60);
		try
		{
			Element.sendKeys(Keys.LEFT_CONTROL.toString() + 'a');
		}
		catch (Exception ex)
		{
			// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
		}
	}

	public void SendEnter()
	{
		Element = CheckElementConditions(60);
		try
		{
			Element.sendKeys(Keys.ENTER);
		} 
		catch (Exception ex) 
		{
			// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
		}
	}

	public void Clear() 
	{
		Element = CheckElementConditions(60);
		try 
		{
			Element.clear();
		} 
		catch (Exception ex) 
		{
			// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
		}
	}

	public void SendKeys(String text, boolean clear) 
	{
		Element = CheckElementConditions(60);

		if (clear) 
		{
			try
			{
				Element.clear();
				Element.sendKeys(text);
			}
			catch (Exception ex)
			{
				// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
			}
		}
		else 
		{
			try 
			{
				Element.sendKeys(text);
			} 
			catch (Exception ex)
			{
				// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
			}
		}
	}

	public String GetAttribute(String attribute) 
	{
		Element = CheckElementConditions(15);
		return Element.getAttribute(attribute);
	}

	public String Text() {
		try 
		{
			Element = CheckElementConditions(60);
			String result = "";
			result = Element.getText();
			if (result == "")
			{
				result = Element.getAttribute("value");
			}

			return result;
		} 
		catch (Exception e) 
		{
			try 
			{
				Browser.Refresh();
				Element = CheckElementConditions(60);
				String result = "";
				result = Element.getText();
				if (result == "") 
				{
					result = Element.getAttribute("value");
				}

				return result;
			}
			catch (Exception ex) 
			{
				// здесь был логгер, но его нет желания доделывать (так же везде должно быть)
				return "";
			}
		}
	}

	private WebElement CheckElementConditions(int timeOut) 
	{
		if (AlreadyFinded) 
		{
			return Element;
		}
		if (OnlyInDom) 
		{
			AlreadyFinded = true;
			return Browser.FindHiddenElement(XPath, timeOut);
		} 
		else 
		{
			AlreadyFinded = true;
			return Browser.FindElement(XPath);
		}
	}

	public void SetFocus() 
	{
		Element = CheckElementConditions(15);

		Browser.ScrollElementToCenter(Element.getLocation());
	}

	public void JClick() 
	{
		Element = CheckElementConditions(60);
		Browser.ExecuteJavaScript("(arguments[0]).click();", (Object)Element);
	}
}

