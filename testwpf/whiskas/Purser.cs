using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System.Diagnostics;

namespace testwpf.whiskas
{
   public class Purser
   {
      static public List<Product> Start()
      {
         var driverService = PhantomJSDriverService.CreateDefaultService();
         driverService.HideCommandPromptWindow = true;

         PhantomJSDriver Driv = new PhantomJSDriver(driverService);
         Driv.Navigate().GoToUrl("https://market.yandex.ru");
         IWebElement Search = Driv.FindElement(By.Id("header-search"));
         Search.SendKeys(MainWindow.cfg.findProduct);
         Search.SendKeys(Keys.Return);
         string url = Driv.Url;
         url += "&priceto=2000&how=aprice";
         Driv.Navigate().GoToUrl(url);
         var document = new HtmlDocument();
         document.LoadHtml(Driv.PageSource);

         List<Product> listProduct = new List<Product>();

         HtmlNodeCollection nodeCollection = document.DocumentNode.SelectNodes("//div[@class='n-snippet-card2__title']/a");
         HtmlNodeCollection nodeCollection2 = document.DocumentNode.SelectNodes("//div[@class='price']");

         if (nodeCollection != null)
         {
            int i = -1;
            foreach (HtmlNode node in nodeCollection)
            {
               string price = nodeCollection2[++i].InnerText;
               listProduct.Add(new Product(node.GetAttributeValue("title", ""), "https://market.yandex.ru" + node.GetAttributeValue("href", ""), price.Remove(price.Length - 7, 7) ));
            }
         }

         return listProduct;
      }
   }
}
