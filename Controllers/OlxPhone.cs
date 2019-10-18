using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Linq;

namespace WebAppParser.Controllers
{
    public class OlxPhone
    {
        public string ParseNumber(string url)
        {
            string phone;

            var service = FirefoxDriverService.CreateDefaultService();
            IWebDriver driver = new FirefoxDriver();
            driver.Url = url;
            IWebElement element = driver.FindElement(By.ClassName("cookie-close"));
            element.Click();
            element = driver.FindElement(By.ClassName("spoiler"));
            element.Click();

            var htmlDocument = driver.PageSource;
            driver.Close();
            int phoneIndex = htmlDocument.IndexOf("data-phone");
            phone = htmlDocument.Substring(phoneIndex,30);
            phone = phone.Remove(phone.IndexOf(">"));
            phone = new String(phone.Where(Char.IsDigit).ToArray());

        return phone;

        }





    }
}
