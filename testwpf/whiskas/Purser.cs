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
      static public Dictionary<string, string> CategoryList;

      static public List<Product> Start(Request findProduct = null, string pars = null)
      {
         if (findProduct == null)
         {
            findProduct = new Request();
            findProduct.requestName = MainWindow.cfg.findProduct;
         }
            

         var driverService = PhantomJSDriverService.CreateDefaultService();
         driverService.HideCommandPromptWindow = true;

         PhantomJSDriver Driv = new PhantomJSDriver(driverService);
         if (pars == null)
         {
            Driv.Navigate().GoToUrl("https://market.yandex.ru");
            IWebElement Search = Driv.FindElement(By.Id("header-search"));
            Search.SendKeys(findProduct.requestName);
            Search.SendKeys(Keys.Return);
            string url = Driv.Url;
            url += "&how=aprice&viewtype=list";
            Driv.Navigate().GoToUrl(url);
         }
         else
         {
            Driv.Navigate().GoToUrl(CategoryList[pars]);
         }

         var document = new HtmlDocument();
         document.LoadHtml(Driv.PageSource);

         HtmlNode AllCategory = document.DocumentNode.SelectSingleNode("//a[@class='link link_theme_normal']");
         if (AllCategory != null)
            Driv.Navigate().GoToUrl("https://market.yandex.ru" + AllCategory.GetAttributeValue("href", ""));
         HtmlNode moreCategory = document.DocumentNode.SelectSingleNode("//div[@class='_2BLXswkhGO']/span");
         if (moreCategory != null)
         {
            IWebElement Search2 = Driv.FindElement(By.XPath("//div[@class='_2BLXswkhGO']/span"));
            Search2.Click();
         }

         //document.LoadHtml(Driv.PageSource);
         List<Product> listProduct = new List<Product>();

         CategoryList = new Dictionary<string, string>();
         HtmlNodeCollection Category = document.DocumentNode.SelectNodes("//div[@class='SMIUZQVy8Y']//a");
         HtmlNodeCollection nodeCollection = document.DocumentNode.SelectNodes("//div[@class='n-snippet-card2__title']/a");
         HtmlNodeCollection nodeCollection2 = document.DocumentNode.SelectNodes("//div[@class='n-snippet-card2__main-price']");
         if (Category != null)
            foreach (HtmlNode node in Category)
            {

               CategoryList.Add(node.FirstChild.InnerText, "https://market.yandex.ru" + node.GetAttributeValue("href", "").Replace("amp;", "") + "&how=aprice&viewtype=list");
            }

         if (nodeCollection != null)
         {
            int i = 0;
            foreach (HtmlNode node in nodeCollection)
            {
               string price = nodeCollection2[i].InnerText;
               if (nodeCollection2[i].InnerText != "Нет в продаже")
               {
                  price = price.Remove(price.Length - 7, 7);
               }

               string href = node.GetAttributeValue("href", "").Replace("amp;", "");
               if (href.Contains("market-click2"))
                  href = "https:" + href;
               else
                  href = "https://market.yandex.ru" + href;
               listProduct.Add(new Product(node.GetAttributeValue("title", ""), href, price));
               i++;
            }
         }
         return listProduct;
      }
   }
}
