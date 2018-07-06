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
         if ( File.Exists(fileName) )
         {
            setting = (Settings)XmlSaver.Read(fileName, typeof(Settings));
         }

         MainWindow.cfg = setting;
      }
   }
}
