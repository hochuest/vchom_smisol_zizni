using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testwpf;

namespace testwpf.whiskas
{
   public static class SaverSettings
   {

      public static string fileName = "settings.xml";

      public static void Save()
      {
         XmlSaver.Save(fileName, MainWindow.cfg);
      }

      public static void Refresh()
      {
         var setting = new Settings();
         if (File.Exists(fileName))
         {
            setting = (Settings)XmlSaver.Read(fileName, typeof(Settings));
         }
         else
         {
            setting.findProduct = "";
            setting.mailLogin = "nim20101@yandex.ru";
            setting.mailPass = "madama98";

            setting.message_header = "заголовок сообщения";
            setting.letter_subject = "тема сообщения";
            setting.before_the_message = "Поступили новые товары: ";
            setting.after_the_message = " (c) Система оповещений о новых товарах";
         }

         MainWindow.cfg = setting;
      }
   }
}
