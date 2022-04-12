using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class SearchPurchaseResult
    {
        public bool isSuccessful { get; set; }
        public List<SearchPurchaseResult> PurchaseList { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public int Cost { get; set; }
        public Sector Sector { get; set; }
        public DateTime ShopDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
