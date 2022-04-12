using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.DataAccessLayer
{
    public interface IPurchaseDAO
    {
        void CreatePurchase(Purchase purchase);
        List<Purchase> GetPurchases(Purchase purchase);
        void EditPurchase(Purchase purchase);
    }
}
