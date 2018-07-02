using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace testwpf.whiskas
{
   class getID
   {
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
      public static void Go(ComboBox CB, Dictionary<string, string> ID)
      {
         string url = "https://api.content.market.yandex.ru/v2/categories/?geo_id=195&count=30&fields=ALL&format=XML";//API запрос, все категории доступные для Ульска
         String res = SendAPIRequest(url);//ответ приходит в формате XML

         //Разбор XML
         var document = new HtmlAgilityPack.HtmlDocument();
         document.LoadHtml(res);
         HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//category");//XPATH запрос выбрать все категории
         foreach (HtmlNode item in nodes)
         {
            //Получаем аттрибуты - имя и айдишник категории
            CB.Items.Add(item.GetAttributeValue("fullName", ""));
            ID.Add(item.GetAttributeValue("fullName", ""), item.GetAttributeValue("id", ""));

            //Тоже самое для подкатегорий
         }
      }
   }
}
