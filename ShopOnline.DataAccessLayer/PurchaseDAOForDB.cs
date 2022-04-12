using ShopOnline.DataAccessLayer.Model;
using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.DataAccessLayer
{
    public class PurchaseDAOForDB : IPurchaseDAO
    {
        public void CreatePurchase(Purchase purchase)
        {
            Purchases purchaseToAdd = new Purchases()
            {
                UserId = (int)(purchase?.UserId),
                ProductId = (int)(purchase?.ProductId),
                ShopDate = (DateTime)(purchase.ShopDate),
                ReturnDate = purchase.ReturnDate.HasValue ? (DateTime?)purchase.ReturnDate.Value : purchase.ReturnDate = null
            };

            using (var context = new ShopOnlineDBEntities())
            {
                context.Purchases.Add(purchaseToAdd);
                try
                {
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }

        public void EditPurchase(Purchase purchase)
        {
            using (var context = new ShopOnlineDBEntities())
            {
                context.Database.Log = Console.WriteLine;
                var purchaseToUpdate = context.Purchases.Find(purchase.PurchaseId);
                purchaseToUpdate.ReturnDate = purchase.ReturnDate;

                context.Entry(purchaseToUpdate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public List<Purchase> GetPurchases(Purchase purchase)
        {
            List<Purchases> purchaseList = new List<Purchases>();
            List<Purchase> purchaseListToGet = new List<Purchase>();

            Purchases purchaseToGet = new Purchases()
            {
                ProductId = (int)(purchase?.ProductId),
                UserId = (int)(purchase?.UserId)
                //ShopDate = (DateTime)purchase.ShopDate
            };

            using(var context = new ShopOnlineDBEntities())
            {
                purchaseList = context.Purchases.Where(p => (purchaseToGet.ProductId == 0 || p.ProductId == purchaseToGet.ProductId) && 
                (purchaseToGet.UserId == 0 || p.UserId == purchaseToGet.UserId)).ToList();
            }

            foreach(var p in purchaseList)
            {
                Purchase purchaseInList = new Purchase()
                {
                    PurchaseId = p.PurchaseId,
                    ProductId = p.ProductId,
                    ReturnDate = p.ReturnDate,
                    ShopDate = p.ShopDate,
                    UserId = p.UserId
                };

                purchaseListToGet.Add(purchaseInList);
            }

            return purchaseListToGet;
        }
    }
}
