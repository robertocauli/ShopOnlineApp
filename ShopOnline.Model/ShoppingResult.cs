using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class ShoppingResult
    {
        public Product ProductPurchased { get; set; }
        public bool isPurchase { get; set; }
        public bool productDoesNotExist { get; set; }
    }
}
