using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;
using System.Xml.Linq;
using System.Net.Mail;

using testwpf;
using System.Diagnostics;

namespace testwpf.whiskas
{
   public class Purser
   {
      //имя продукта берем из формы, айдишники всех категорий мы знаем
      private static string CreateURL(String product, String id)
      {
         String result = "https://market.yandex.ru/catalog/" + id + "/list?text=" + product + "&local-offers-first=0&priceto=2000&how=aprice";
         return result;
      }

      private static string LoadPage(string url)
      {
         var request = (HttpWebRequest)WebRequest.Create(url);
         String result = ReadAnswer(request);
         return result;
      }

      private static string ReadAnswer(HttpWebRequest request)
      {
         String result;
         using (WebResponse response = request.GetResponse())
         {
            using (Stream stream = response.GetResponseStream())
            {
               using (StreamReader reader = new StreamReader(stream))
               {
                  result = reader.ReadToEnd();
               }
            }
         }
         return result;
      }

      private static string SendAPIRequest(string url)
      {
         string res;
         var request = (HttpWebRequest)WebRequest.Create(url);//посылает запрос на сервак
         request.Headers.Add("Authorization: ce235f24-31e4-4f82-a1e3-77553daf404c");//авторизационный ключ
         res = ReadAnswer(request);
         return res;
      }

      private static void GetCategoryList()
      {
         string url = "https://api.content.market.yandex.ru/v2/categories/?geo_id=195&count=30&fields=ALL&format=XML";//API запрос, все категории доступные для Ульска
         String res = SendAPIRequest(url);//ответ приходит в формате XML

         //Разбор XML
         var document = new HtmlDocument();
         document.LoadHtml(res);
         HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//category");//XPATH запрос выбрать все категории
         foreach (HtmlNode item in nodes)
         {
            //Получаем аттрибуты - имя и айдишник категории
            Debug.WriteLine("Name = " + item.GetAttributeValue("name", "") +
              " FullName = " + item.GetAttributeValue("fullName", "") +
                " - id = " + item.GetAttributeValue("id", ""));
            String id = item.GetAttributeValue("id", "");

            //Тоже самое для подкатегорий
            string tempUrl = "https://api.content.market.yandex.ru/v2/categories/" + id + "/children/?geo_id=195&count=30&fields=ALL&format=XML";
            String temp = SendAPIRequest(tempUrl);

            var doc = new HtmlDocument();
            doc.LoadHtml(temp);
            HtmlNodeCollection children = doc.DocumentNode.SelectNodes("//category");
            foreach (HtmlNode child in children)
            {
              // Console.WriteLine("\t->Name = " + child.GetAttributeValue("name", "") +
             //  " FullName = " + child.GetAttributeValue("fullName", "") +
              // " - id = " + child.GetAttributeValue("id", ""));
            }
         }
         //Console.ReadKey();
      }

      static public List<Product> Start()
      {
         //GetCategoryList();
         String link = CreateURL(MainWindow.cfg.findProduct, MainWindow.cfg.categoryId);
         var pageContent = LoadPage(link);
         var document = new HtmlDocument();
         document.LoadHtml(pageContent);

         //Console.WriteLine(pageContent);

         HtmlNodeCollection nodeCollection = document.DocumentNode.SelectNodes("//div[@class='n-snippet-card2__title']/a");
         HtmlNodeCollection nodeCollection2 = document.DocumentNode.SelectNodes("//div[@class='price']");

         List<Product> listProduct = new List<Product>();

         if (nodeCollection != null)
         {
            int i = 0;
            foreach (HtmlNode node in nodeCollection)
            {
               string description = node.GetAttributeValue("title", "") + " " + "https://market.yandex.ru" + node.GetAttributeValue("href", "");
               string price = nodeCollection2[i].InnerText;

               Debug.WriteLine(description + " Цена " + price.Remove(price.Length - 2, 2) + " Руб.");

               listProduct.Add(new Product(node.GetAttributeValue("title", ""), "https://market.yandex.ru" + node.GetAttributeValue("href", "") , price.Remove(price.Length - 2, 2)));
               
               i++;
            }
         }

         return listProduct;

         //Console.Read();
      }
   }
}
