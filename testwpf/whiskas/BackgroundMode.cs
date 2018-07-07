using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace testwpf.whiskas
{
   public static class BackgroundMode
   {
      static Func<Request, string, Request> GetListUrlNew;
      static Func<List<Request>> GetListUrlDB;
      static Action<List<Request>> AddDB;
      static Action<List<Request>> SendMailMethod;

      static public void Start(Func<Request, string, Request> _GetListUrlNew, Func<List<Request>> _GetListUrlDB, Action<List<Request>> _AddDB, Action<List<Request>> _SendMailMethod)
      {

         GetListUrlNew = _GetListUrlNew;
         GetListUrlDB = _GetListUrlDB;
         AddDB = _AddDB;
         SendMailMethod = _SendMailMethod;

         //Body();

         Timer timer = new Timer(60000);
         timer.Elapsed += async (sender, e) => await Task.Factory.StartNew(() => {
            if ((MainWindow.cfg.hour == DateTime.Now.Hour) &&
               (MainWindow.cfg.minute == DateTime.Now.Minute))
            {
               Body();
            }
         });
         timer.Start();


      }

      static void Body()
      {
         List<Request> newRequests = new List<Request>();
         List<Request> listProductDB = GetListUrlDB?.Invoke();

         foreach ( var request in listProductDB )
         {
            var newProducts = new List<Product>();
            List<Product> listProductNew = GetListUrlNew?.Invoke(request, null).ListProduct;

            foreach (Product prod in listProductNew)
            {
               if (request.ListProduct.Find(x => x.name == prod.name && x.price == prod.price) == null)
               {
                  newProducts.Add(prod);
               }
            }

            if (newProducts.Count > 0 )
            {
               newRequests.Add(new Request(request.requestName, newProducts, request.minPrice, request.maxPrice));
            }
         }

         if ( newRequests.Count > 0 )
         {
            AddDB(newRequests);
            SendMailMethod?.Invoke(newRequests);
         }
            
      }

   }
}
