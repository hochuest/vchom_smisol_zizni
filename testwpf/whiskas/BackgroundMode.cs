using System;
using System.Collections.Generic;
using System.Threading;

namespace testwpf.whiskas
{
   public static class BackgroundMode
   {
      static Func<Request, string, List<Product>> GetListUrlNew;
      static Func<List<Request>> GetListUrlDB;
      static Action<List<Request>> AddDB;
      static Action<List<Request>> SendMailMethod;

      static public void Start(Func<Request, string, List<Product>> _GetListUrlNew, Func<List<Request>> _GetListUrlDB, Action<List<Request>> _AddDB, Action<List<Request>> _SendMailMethod)
      {

         GetListUrlNew = _GetListUrlNew;
         GetListUrlDB = _GetListUrlDB;
         AddDB = _AddDB;
         SendMailMethod = _SendMailMethod;

         //Body();

         var timer = new Timer((obj) => {
            if (( MainWindow.cfg.hour == DateTime.Now.Hour) &&
               ( MainWindow.cfg.minute == DateTime.Now.Minute))
            {
               Body();
            }
         }, null, 0, 60000);

      }

      static void Body()
      {
         List<Request> newRequests = new List<Request>();
         List<Request> listProductDB = GetListUrlDB?.Invoke();

         foreach ( var request in listProductDB )
         {
            var newProducts = new List<Product>();
            List<Product> listProductNew = GetListUrlNew?.Invoke(request, null);

            foreach (Product prod in listProductNew)
            {
               if (request.ListProduct.Find(x => x.url == prod.url) == null)
               {
                  newProducts.Add(prod);
               }
            }

            if ( newRequests.Count != 0 )
            {
               newRequests.Add(new Request(request.requestName, newProducts, request.minPrice, request.maxPrice));
            }
         }

         if ( newRequests.Count != 0 )
         {
            AddDB(newRequests);
            //SendMailMethod?.Invoke(newRequests);
         }
            
      }

   }
}
