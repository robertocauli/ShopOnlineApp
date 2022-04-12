using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime? ShopDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
