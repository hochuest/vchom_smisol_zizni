using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace testwpf.whiskas
{
   class UseDB
   {
      static string filename = "tipodb.xml";

      public static void addDB(Product p)
      {
         List<Product> db = new List<Product>();
         if ( File.Exists(filename) )
            db = GetList();
         db.Add(p);
         XmlSaver.Save( filename, (object)(db) );
      }

      public static List<Product> GetList()
      {
         return (List<Product>)(XmlSaver.Read(filename));
      }
   }
}
