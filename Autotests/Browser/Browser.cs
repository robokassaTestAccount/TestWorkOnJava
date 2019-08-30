using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace Core
{
    public class Browser
    {
        private string _mainWindowHandler { get; set; }
        private IWebDriver Driver { get; set; }
        public Browser()
        {

        }

        public void StartWebDriver()
        {
            Driver = StartChrome();

            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            Driver.Manage().Window.Maximize();
            _mainWindowHandler = Driver.CurrentWindowHandle;
        }

        private ChromeDriver StartChrome()
        {
            string dir = System.IO.Directory.GetCurrentDirectory();
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", @"C:\TestData");
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", "false");
            chromeOptions.AddUserProfilePreference("acceptInsecureCerts", "true");
            chromeOptions.AddArguments("disable-extensions");
            chromeOptions.AddArguments("ignore-certificate-errors", "ignore-urlfetcher-cert-requests", "allow-insecure-localhost");
            return new ChromeDriver(dir, chromeOptions);
        }

        public IWebElement FindElement(string XPath)
        {
            WaitForAjaxComplete(5);
            IWebElement element;

            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(Driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(1)
            };
            try
            {
                wait.IgnoreExceptionTypes(typeof(Exception));
                element = wait.Until(condition: SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(XPath)));
            }
            catch
            {
                return null;
            }

            return element;
        }

        public List<IWebElement> FindElements(string XPath)
        {
            WaitForAjaxComplete(5);
            List<IWebElement> elements = new List<IWebElement>();

            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(Driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(1)
            };
            wait.IgnoreExceptionTypes(typeof(Exception));
            try
            {
                ReadOnlyCollection<IWebElement> finded = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(XPath)));
                foreach (IWebElement elem in finded)
                {
                    elements.Add(elem);
                }
            }
            catch
            {
                return null;
            }

            return elements;
        }

        public List<IWebElement> FindHiddenElements(string XPath)
        {
            WaitForAjaxComplete(5);
            List<IWebElement> elements = new List<IWebElement>();

            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(Driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(1)
            };
            wait.IgnoreExceptionTypes(typeof(Exception));
            Func<IWebDriver, ReadOnlyCollection<IWebElement>> finder = new Func<IWebDriver, ReadOnlyCollection<IWebElement>>(x => x.FindElements(By.XPath(XPath)));
            try
            {
                ReadOnlyCollection<IWebElement> finded = wait.Until(finder);
                foreach (IWebElement elem in finded)
                {
                    elements.Add(elem);
                }
            }
            catch
            {
                return null;
            }

            return elements;
        }
        private List<IWebElement> CheckConditions(string XPath)
        {
            ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.XPath(XPath));

            List<IWebElement> result = new List<IWebElement>();

            foreach (IWebElement elem in elements)
            {
                if (elem.Enabled && elem.Displayed)
                {
                    result.Add(elem);
                }
            }

            return result;
        }


        public IWebElement FindHiddenElement(string XPath, int timeOut = 15)
        {
            WaitForAjaxComplete(5);
            IWebElement element;

            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(Driver)
            {
                Timeout = TimeSpan.FromSeconds(timeOut),
                PollingInterval = TimeSpan.FromMilliseconds(1)
            };
            wait.IgnoreExceptionTypes(typeof(Exception));
            try
            {
                element = wait.Until(condition: SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(XPath)));
            }
            catch
            {
                return null;
            }

            return element;
        }

        public void Navigate(string Url)
        {
            UriBuilder uri = new UriBuilder(Url);
            Driver.Navigate().GoToUrl(uri.Uri);
        }

        public void SwitchToMain()
        {
            Driver.SwitchTo().Window(_mainWindowHandler);
        }
        public void SwitchToNewWindow()
        {
            ReadOnlyCollection<string> handles = Driver.WindowHandles;
            Driver.SwitchTo().Window(handles.Last());
        }

        public IReadOnlyCollection<string> GetHandlers()
        {
            return Driver.WindowHandles;
        }

        public void SwitchToNewWindow(string Handl)
        {
            Driver.SwitchTo().Window(Handl);
        }

        public void CloseActiveWindows()
        {
            ReadOnlyCollection<string> handles = Driver.WindowHandles;
            foreach (string handl in handles)
            {
                handles = Driver.WindowHandles;
                SwitchToNewWindow(handl);
                if (handl != _mainWindowHandler)
                {
                    Driver.Close();
                }
            }
        }

        public void OpenWindow()
        {
            ExecuteJavaScript("window.open('your url','_blank');");
        }

        public void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public Uri Url
        {
            get
            {
                WaitForAjaxComplete(3);
                return new Uri(Driver.Url);
            }
        }

        public string Title
        {
            get
            {
                WaitForAjaxComplete(3);
                return Driver.Title;
            }
        }
        public void Quit()
        {
            Driver.Quit();
        }

        public void AllertAccept()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30) /*timeout in seconds*/);
            if (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent()) != null)
            {
                Driver.SwitchTo().Alert().Accept();
            }
        }

        public object ExecuteJavaScript(string javaScript, params object[] args)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)Driver;
            try
            {
                return javaScriptExecutor.ExecuteScript(javaScript, args);
            }
            catch
            {
                return javaScriptExecutor.ExecuteScript(javaScript, args);
            }
        }
        public void WaitForAjaxComplete(int timeoutInSeconds)
        {
            string firstSource = Driver.PageSource;
            Thread.Sleep(100);
            string secondSource = Driver.PageSource;
            int allTime = 0;
            while(firstSource != secondSource)
            {
                if(allTime >= timeoutInSeconds)
                {
                    break;
                }
                firstSource = Driver.PageSource;
                Thread.Sleep(100);
                secondSource = Driver.PageSource;
                allTime += 100;
            }
        }
        public void ScrollElementToCenter(Point location)
        {
            ScrollToElement(location - new Size(0, GetBrowserWindowInnerHeight() / 2));
        }
        public void ScrollToElement(Point location)
        {
            ExecuteJavaScript($"window.scrollTo({location.X}, {location.Y});");
        }
        public int GetBrowserWindowInnerHeight()
        {
            return int.Parse(ExecuteJavaScript("return window.innerHeight;").ToString());
        }
    }
}
