using System.Collections.ObjectModel;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

public class SeleniumUtilService
{
    private IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private IJavaScriptExecutor _jsExecutor;

    public SeleniumUtilService()
    {
        _driver = new ChromeDriver();
        _jsExecutor = (IJavaScriptExecutor)_driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    public void NavigateToUrl(string path)
    {
        try
        {
            if (_driver == null || !_driver.WindowHandles.Any())
            {
                _driver = new ChromeDriver();
            }
            _driver.Navigate().GoToUrl(path);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while navigating to url: {path}.", ex);
        }
    }

    public bool CheckExistsByXpath(string xpath)
    {
        try
        {
            return _wait.Until(driver => driver.FindElement(By.XPath(xpath)).Enabled && driver.FindElement(By.XPath(xpath)).Displayed);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to identify or verify element: {xpath}.", ex);
        }
    }

    public bool CheckExistsXpathOnHtml(HtmlDocument htmlDoc, string xpath)
    {
        try
        {
            var data = htmlDoc.DocumentNode.SelectSingleNode(xpath);
            return (data != null) ? true : false;
        }
        catch
        {
            return false;
        }
    }

    public void ClickByXpath(string xpath)
    {
        try
        {
            _driver.FindElement(By.XPath(xpath)).Click();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while clicking on element: {xpath}.", ex);
        }
    }

    public IWebElement GetElementLinkByCssSelector(IWebElement element, string cssSelector)
    {
        try
        {
            var link = element.FindElement(By.CssSelector(cssSelector));
            return link;
        }
        catch (Exception ex)
        {
            throw new Exception("Error while getting link by selector.", ex);
        }
    }

    public ReadOnlyCollection<IWebElement> GetWebElementByCssSelector(string cssSelector)
    {
        try
        {
            return _driver.FindElements(By.CssSelector(cssSelector));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while identifying and getting element: {cssSelector}", ex);
        }
    }


    public List<string> GetLinksFromHtmlNode(HtmlNode htmlNode)
    {
        try
        {
            var linkNodes = htmlNode.SelectNodes(".//a");
            List<string> links = new List<string>();
            foreach (var linkNode in linkNodes)
            {
                var href = linkNode.GetAttributeValue("href", string.Empty);
                if (!string.IsNullOrEmpty(href))
                {
                    links.Add(href);
                }
            }
            return links;
        }
        catch (Exception ex)
        {
            throw new Exception("Fail while getting links from html node.", ex);
        }
    }

    public HtmlDocument GetHtmlDocumentFromUrl(string url)
    {
        try
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while getting Html page.", ex);
        }
    }

    public HtmlNode? GetDataFromHtmlDoc(HtmlDocument htmlDoc, string xPath)
    {
        try
        {
            if (this.CheckExistsXpathOnHtml(htmlDoc, xPath))
            {
                return htmlDoc.DocumentNode.SelectSingleNode(xPath);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error while getting element from Html", ex);
        }
    }

    public void JsInputByXpath(string xpath, string value)
    {
        try
        {
            _jsExecutor.ExecuteScript("document.evaluate(arguments[0], document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.value = arguments[1];", xpath, value);
        }
        catch (Exception ex)
        {
            throw new Exception("Error to execute js to send key in the field.", ex);
        }
    }

    public void JsClickByXpath(string xpath, string value)
    {
        try
        {
            var element = _driver.FindElement(By.XPath(xpath));
            _jsExecutor.ExecuteScript("arguments[0].click();", element);
        }
        catch (Exception ex)
        {
            throw new Exception("Error to execute click action by js.", ex);
        }
    }
}
