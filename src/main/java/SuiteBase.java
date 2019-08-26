import java.io.File;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import org.w3c.dom.Document;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import org.junit.After;
import org.junit.Before;

public class SuiteBase 
{
	public Browser browser;
	public Pages pages;
	
	@Before
	public void TestSetup()
	{
		String browserType = GetBrowserType();
		if(browserType != "")
		{
			browser = new Browser(browserType);
		}
		else
		{
			browser = new Browser();
		}		
		browser.StartWebDriver();
		pages = new Pages(browser);
	}
	
	@After
	public void TestCleanUp()
	{
		browser.Quit();
	}
	
	private String GetBrowserType()
	{
		try 
		{
			{
				Path currentRelativePath = Paths.get("");
				String s = currentRelativePath.toAbsolutePath().toString();
				
				Config configs = null;
			    DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
			    DocumentBuilder builder = factory.newDocumentBuilder();
			    Document document = builder.parse(new File(s+"/config.xml"));
			    
			    NodeList browserElements = document.getDocumentElement().getElementsByTagName("Browser");

		        for (int i = 0; i < browserElements.getLength(); i++)
		        {
		            Node Browser = browserElements.item(i);
		            NamedNodeMap attributes = Browser.getAttributes();		            
		            configs = new Config(attributes.getNamedItem("name").getNodeValue());
		        }
		        return configs.GetBrowser();
			}
		} 
		catch (ParserConfigurationException e) 
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		} 
		catch (SAXException e) 
		{
			e.printStackTrace();
		} 
		catch (IOException e) 
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return "";
	}
}
