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
         var db = GetList();
         db.Add(p);
         XmlSaver.Save( filename, db );
      }

      public static List<Product> GetList()
      {
         if ( File.Exists(filename) )
            return (List<Product>)(XmlSaver.Read(filename, typeof(List<Product>)));
         else
            return new List<Product>();
      }
   }
}
