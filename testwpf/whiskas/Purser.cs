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
      static public int Minimum;
      static public int Maximum;

      static public Dictionary<string, string> CategoryList;

      static public HtmlDocument document = new HtmlDocument();

      static public Request Start(Request findRequest, string pars = null)
      {
         var driverService = PhantomJSDriverService.CreateDefaultService();
         driverService.HideCommandPromptWindow = true;

         PhantomJSDriver Driv = new PhantomJSDriver(driverService);
         if (pars == null)
         {
            Driv.Navigate().GoToUrl("https://market.yandex.ru");
            IWebElement Search = Driv.FindElement(By.Id("header-search"));
            Search.SendKeys(findRequest.requestName);
            Search.SendKeys(Keys.Return);
            string url = Driv.Url;
            //мин и макс ретурн их
            document.LoadHtml(Driv.PageSource);
            HtmlNodeCollection Price = document.DocumentNode.SelectNodes("//div[@class='_16hsbhrgAf']/ul/li/p/input");
            if (Price != null)
            {
               Minimum = Convert.ToInt32(Price[0].GetAttributeValue("placeholder", "").Replace(" ", ""));
               Maximum = Convert.ToInt32(Price[1].GetAttributeValue("placeholder", "").Replace(" ", ""));
            }
            if (Minimum <= findRequest.minPrice && Maximum >= findRequest.maxPrice)
               url += $"&how=aprice&viewtype=list&pricefrom={findRequest.minPrice}&priceto={findRequest.maxPrice}";
            else
               url += $"&how=aprice&viewtype=list&pricefrom={Minimum}&priceto={Maximum}";
            Driv.Navigate().GoToUrl(url);
         }
         else
         {
            Driv.Navigate().GoToUrl(CategoryList[pars]);

            document.LoadHtml(Driv.PageSource);
            HtmlNodeCollection Price = document.DocumentNode.SelectNodes("//div[@class='_16hsbhrgAf']/ul/li/p/input");
            if (Price != null)
            {
               Minimum = Convert.ToInt32(Price[0].GetAttributeValue("placeholder", "").Replace(" ", ""));
               Maximum = Convert.ToInt32(Price[1].GetAttributeValue("placeholder", "").Replace(" ", ""));
            }
         }

         document = new HtmlDocument();
         document.LoadHtml(Driv.PageSource);

         HtmlNode AllCategory = document.DocumentNode.SelectSingleNode("//a[@class='link link_theme_normal']");
         if (AllCategory != null)
            Driv.Navigate().GoToUrl("https://market.yandex.ru" + AllCategory.GetAttributeValue("href", "") + $"&how=aprice&viewtype=list&pricefrom ={findRequest.minPrice}&priceto ={findRequest.maxPrice}");
         HtmlNode moreCategory = document.DocumentNode.SelectSingleNode("//div[@class='_2BLXswkhGO']/span");
         if (moreCategory != null)
         {
            IWebElement Search2 = Driv.FindElement(By.XPath("//div[@class='_2BLXswkhGO']/span"));
            Search2.Click();
         }

         Driv.Quit();

         //document.LoadHtml(Driv.PageSource);

         List<Product> ListProduct = new List<Product>();

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
               ListProduct.Add(new Product(node.GetAttributeValue("title", ""), href, price));
               i++;
            }
         }
         
         return new Request( findRequest.requestName, ListProduct, findRequest.minPrice, findRequest.maxPrice );
      }
   }
}
