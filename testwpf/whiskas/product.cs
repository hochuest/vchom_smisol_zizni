using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testwpf.whiskas
{
   public class Product
   {
      public string name { get; set; }
      public string url { get; set; }
      public string price { get; set; }

      public Product() { }

      public Product(string name, string url, string price)
      {
         this.name = name;
         this.url = url;
         this.price = price;
      }
   }
}
