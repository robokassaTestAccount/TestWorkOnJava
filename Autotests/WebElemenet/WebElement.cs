using OpenQA.Selenium;
using Core;
using System;
using LoggerHelperSpace;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace WebElemenet
{
    public class WebElement
    {
        public LoggerHelper Log { private get; set; }
        public Browser Browser { private get; set; }
        public bool OnlyInDom { private get; set; }
        public string XPath { get; set; }
        private IWebElement Element {get; set;}
        public string Description { get; set; }
        private bool AlreadyFinded { get; set; }        

        public WebElement(string XPath = "", string Description = "")
        {
            this.XPath = XPath;
            this.Description = Description;
        }
        public void Click()
        {
            Element = CheckElementConditions();

            try
            {
                Log.WriteInfo($"click() по элементу:\"{Description}\"           с XPath:\"{XPath}\"");
                Element.Click();
            }
            catch(Exception ex)
            {
                Log.WriteError($"Попытка click() по элементу:\"{Description}\"          с XPath:\"{XPath}\" не удалась", ex);                
            }
        }

        public bool Exists(int timeOut = 15)
        {
            try
            {
                OnlyInDom = true;
                Element = CheckElementConditions(timeOut);
                if(Element == null)
                {
                    return false;
                }
                if (Element.Displayed && Element.Enabled)
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
            catch(Exception ex)
            {
                Log.WriteError($"Элемент '{Description}' с XPath '{XPath}' не найден", ex);
                return false;
            }
        }
        
        public WebElement ConvertToWebElement(IWebElement elem)
        {
            Element = elem;
            AlreadyFinded = true;
            return this;
        }
        internal enum SelectTypes
        {
            ByValue,
            ByText
        }
        private void SelectCommon(string option, SelectTypes selectType)
        {
            Element = CheckElementConditions();

            Log.WriteInfo($"Выбор элемента {Description} с XPath {XPath}");

            switch (selectType)
            {
                case SelectTypes.ByValue:
                    new SelectElement(Element).SelectByValue(option);
                    return;
                case SelectTypes.ByText:
                    new SelectElement(Element).SelectByText(option);
                    return;
                default:
                    throw new Exception($"Unknown select type: {selectType}.");
            }
        }
        public void SelectByText(string optionText)
        {
            SelectCommon(optionText, SelectTypes.ByText);
        }
        public void CheckText()
        {
            Element = CheckElementConditions();
            try
            {
                Log.WriteInfo($"Выделение текста элемента:\"{Description}\"         с XPath:\"{XPath}\"");
                Element.SendKeys(Keys.LeftControl + 'a');
            }
            catch(Exception ex)
            {
                Log.WriteError($"Выделение текста элемента:\"{Description}\"         с XPath:\"{XPath}\" не удалась", ex);
            }
        }
        public void SendEnter()
        {
            Element = CheckElementConditions();
            try
            {
                Log.WriteInfo($"Пердача элементу нажатие клавиши Enter:\"{Description}\"         с XPath:\"{XPath}\"");
                Element.SendKeys(Keys.Enter);
            }
            catch (Exception ex)
            {
                Log.WriteError($"Пердача элементу нажатие клавиши Enter:\"{Description}\"         с XPath:\"{XPath}\" не удалась", ex);
            }
        }
        public void Clear()
        {
            Element = CheckElementConditions();
            try
            {
                Log.WriteInfo($"Очистка элемента:\"{Description}\"         с XPath:\"{XPath}\"");
                Element.Clear();
            }
            catch(Exception ex)
            {
                Log.WriteError($"Очистка элемента:\"{Description}\"         с XPath:\"{XPath}\" не удалась", ex);
            }
        }
        public void SendKeys(string text, bool clear = false)
        {
            Element = CheckElementConditions();

            if(clear)
            {
                try
                {
                    Log.WriteInfo($"Запись сообщения:\"{text}\"         в элемент:\"{Description}\"         с XPath:\"{XPath}\" с очисткой элемента");
                    Element.Clear();
                    Element.SendKeys(text);                    
                }
                catch(Exception ex)
                {
                    Log.WriteError($"Запись сообщения:\"{text}\"            в элемент:\"{Description}\"         с XPath:\"{XPath}\" с очисткой элемента не удалась", ex);
                }
            }
            else
            {
                try
                {
                    Log.WriteInfo($"Запись сообщения:\"{text}\"         в элемент:\"{Description}\"         с XPath:\"{XPath}\"");
                    Element.SendKeys(text);
                }
                catch(Exception ex)
                {
                    Log.WriteError($"Запись сообщения:\"{text}\"            в элемент:\"{Description}\"         с XPath:\"{XPath}\"", ex);
                }                
            }
        }

        public string GetAttribute(string attribute)
        {
            Element = CheckElementConditions();
            Log.WriteInfo($"Запрос атрибута у элемента {Description} с XPath {XPath}");
            return Element.GetAttribute(attribute);
        }

        public string Text
        {
            get
            {
                try
                {
                    Element = CheckElementConditions();
                    Log.WriteInfo($"Запрос текста у элемента {Description} с XPath {XPath}");
                    string result = "";
                    result = Element.Text;
                    if(result == "")
                    {
                        result = Element.GetAttribute("value");
                    }

                    return result;
                }
                catch
                {
                    try
                    {
                        Browser.Refresh();
                        Element = CheckElementConditions();
                        Log.WriteInfo($"Запрос текста у элемента {Description} с XPath {XPath}");
                        string result = "";
                        result = Element.Text;
                        if (result == "")
                        {
                            result = Element.GetAttribute("value");
                        }

                        return result;
                    }
                    catch(Exception ex)
                    {
                        Log.WriteError($"Ошибка при получении текста элемента",ex);
                        return "";
                    }
                }
            }
        }

        private IWebElement CheckElementConditions(int timeOut = 15)
        {
            if(AlreadyFinded)
            {
                return Element;
            }
            if(OnlyInDom)
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
            Element = CheckElementConditions();

            Browser.ScrollElementToCenter(Element.Location);
        }

        public void ExecuteJQueryString(string str)
        {
            Element = CheckElementConditions();

            Browser.ExecuteJavaScript($"$(arguments[0]).{str};", Element);
        }

        public void JClick()
        {
            Element = CheckElementConditions();
            Browser.ExecuteJavaScript("$(arguments[0]).click();", Element);
        }
    }
    public class WebElementCollection
    {
        private string Description { get; set; }
        private bool Hidden { get; set; }
        public WebElementCollection(string Description = "", bool hidden = false)
        {
            this.Description = Description;
            Hidden = hidden;
        }
        public List<WebElement> GetCollection(string XPath, Browser browser, LoggerHelper logger)
        {            
            List<WebElement> Result = new List<WebElement>();
            List<IWebElement> firstElems;
            if (Hidden)
            {
                firstElems = browser.FindHiddenElements(XPath);
            }
            else
            {
                firstElems = browser.FindElements(XPath);
            }
            if (firstElems == null)
            {
                return null;
            }
            foreach (IWebElement Element in firstElems)
            {
                WebElement webElement = new WebElement("", Description)
                {
                    Browser = browser,
                    Log = logger,
                    OnlyInDom = Hidden
                };
                webElement = webElement.ConvertToWebElement(Element);
                Result.Add(webElement);
            }
            return Result;
        }
    }
}
