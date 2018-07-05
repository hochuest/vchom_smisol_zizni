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

      public static void AddRange(List<Request> newListRequest)
      {
         var db = GetList();

         foreach ( var v in newListRequest)
            db.Find(x => x.requestName == v.requestName).ListProduct.AddRange(v.ListProduct);

         XmlSaver.Save(filename, db);
      }

      public static void Add(Request newRequest)
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
               if (h.ListProduct.Find(x => x.url == v.url) == null)
               {
                  h.ListProduct.Add(v);
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
