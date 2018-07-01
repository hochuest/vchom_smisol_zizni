using System;
using System.Collections.Generic;
using System.Threading;

namespace testwpf.whiskas
{
   public static class BackgroundMode
   {
      static Func<List<Product>> GetListUrlNew;
      static Func<List<Product>> GetListUrlDB;
      static Action<Product> AddDB;
      static Action<List<Product>> SendMailMethod;

      static int hour = 10; // нужн изменять через конфиг
      static int minute = 30; // нужн изменять через конфиг

      static public void Start(Func<List<Product>> _GetListUrlNew, Func<List<Product>> _GetListUrlDB, Action<Product> _AddDB, Action<List<Product>> _SendMailMethod)
      {
         GetListUrlNew = _GetListUrlNew;
         GetListUrlDB = _GetListUrlDB;
         AddDB = _AddDB;
         SendMailMethod = _SendMailMethod;

         var timer = new Timer((obj) => {
            if ((hour == DateTime.Now.Hour) &&
               ( minute == DateTime.Now.Minute))
            {
               Body();
            }

         }, null, 0, 60000);

      }

      static void Body()
      {
         List<Product> listProductNew = GetListUrlNew?.Invoke();
         List<Product> listProductDB = GetListUrlDB?.Invoke();

         List<Product> newProduct = new List<Product>();

         // Ищем на совпадение между новым списком и списком из бд, возможно надо переделать под 
         foreach (Product prod in listProductNew) 
         {
            if (listProductDB?.Find(x => x.url == prod.url) == null)
            {
               newProduct.Add(prod);
               AddDB?.Invoke(prod);
            }
         }

         SendMailMethod?.Invoke(newProduct);
      }

   }
}
