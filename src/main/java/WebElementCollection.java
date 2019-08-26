import java.util.ArrayList;

import org.openqa.selenium.WebElement;

public class WebElementCollection
    {
        private String Description;
        private Boolean Hidden;
        public WebElementCollection(String Description, boolean hidden)
        {
            this.Description = Description;
            Hidden = hidden;
        }
        public ArrayList<SimpleWebElement> GetCollection(String XPath, Browser browser)
        {            
        	ArrayList<SimpleWebElement> Result = new ArrayList<SimpleWebElement>();
        	ArrayList<WebElement> firstElems = null;
            if (Hidden)
            {
                //firstElems = browser.FindHiddenElements(XPath);
            }
            else
            {
                firstElems = browser.FindElements(XPath);
            }
            if (firstElems == null)
            {
                return null;
            }
            for(WebElement Element : firstElems)
            {
            	SimpleWebElement webElement = new SimpleWebElement("", Description);
            	webElement.Browser = browser;
            	webElement.OnlyInDom = Hidden;
                webElement = webElement.ConvertToSimpleWebElement(Element);
                Result.add(webElement);
            }
            return Result;
        }
    }
