using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testwpf.whiskas
{
   public class Settings
   {
      public string findProduct { get; set; }
      public string mailLogin = "nim20101@yandex.ru";
      public string mailPass = "madama98";

      public string message_header = "заголовок сообщения";
      public string letter_subject = "тема сообщения";
      public string before_the_message = "Поступили новые товары: \n";
      public string after_the_message = " (c) Система оповещений о новых товарах";

      public string recipientOfLetters { get; set; }

      public int hour { get; set; }
      public int minute { get; set; }

      public static string fileName = "settings.xml";

      public static void Save()
      {
         //XmlSaver.Save( );
      }

      public static void Refresh()
      {

      }

   }
}
