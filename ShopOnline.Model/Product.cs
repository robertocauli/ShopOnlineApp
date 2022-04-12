using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Brand { get; set; }
        public int Cost { get; set; }
        public Sector Sectors { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}
