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

      public static void addRange(List<Request> list)
      {
         var db = GetList();

         foreach ( var dd in list )
            db.Find(x => x.requestName == dd.requestName).ListProduct.AddRange(dd.ListProduct);

         XmlSaver.Save(filename, db);
      }

      public static void add(Request newRequest)
      {
         var db = GetList();

         Request h = db.Find(x => x.requestName == newRequest.requestName);
         if (h == null)
         {
            db.Add(newRequest);
         }
         else
         {
            foreach (var v in newRequest.ListProduct)
            {
               if (h.ListProduct.Find(x => x.url == v.url) != null)
               {
                  /// ...
               }
            }
            
            
            
         }

         XmlSaver.Save(filename, db);
      }

      public static List<Request> GetList()
      {
         if ( File.Exists(filename) )
            return (List<Request>)(XmlSaver.Read(filename, typeof(List<Request>)));
         else
            return new List<Request>();
      }
   }
}
