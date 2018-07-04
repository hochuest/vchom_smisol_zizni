using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testwpf.whiskas
{
   public class Request
   {
      public string requestName { get; set; }
      public List<Product> ListProduct { get; set; }
      public int minPrice { get; set; }
      public int maxPrice { get; set; }

      Request() { }

      public Request(string requestName, List<Product> ListProduct, int minPrice , int maxPrice)
      {
         this.requestName = requestName;
         this.ListProduct = ListProduct;
         this.minPrice = minPrice;
         this.maxPrice = maxPrice;
      }
   }
}
