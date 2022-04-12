using ShopOnline.DataAccessLayer.Model;
using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.DataAccessLayer
{
    public class ProductDAOForDB : IProductDAO
    {
        public void CreateProduct(Product product)
        {

            Products productToAdd = new Products()
            {
                ProductName = product.ProductName,
                Brand = product.Brand,
                Cost = product.Cost,
                Sector = (int)product.Sectors,
                Quantity = product.Quantity
            };

            using (var context = new ShopOnlineDBEntities())
            {
                context.Products.Add(productToAdd);
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int idProduct)
        {
            using (var context = new ShopOnlineDBEntities())
            {
                var productToDelete = context.Products.Find(idProduct);
                context.Products.Remove(productToDelete);
                
                
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var context = new ShopOnlineDBEntities())
            {
                Products productToUpdate = new Products();
                context.Database.Log = Console.WriteLine;

                productToUpdate = context.Products.Find(product.ProductId);
                productToUpdate.ProductName = product.ProductName;
                productToUpdate.Brand = product.Brand;
                productToUpdate.Cost = product.Cost;
                productToUpdate.Sector = (int)product.Sectors;
                productToUpdate.Quantity = product.Quantity;

                context.Entry(productToUpdate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public List<Product> GetProducts(Product product)
        {
            List<Products> products = new List<Products>();
            List<Product> productsToGet = new List<Product>();

            Products productToSearch = new Products()
            {
                ProductName = product.ProductName,
                Brand = product.Brand,
                Cost = product.Cost,
                Sector = (int)product.Sectors,
                Quantity = product.Quantity,
                ProductId = product.ProductId
            };
            if (productToSearch.ProductId == 0)
            {
                using (var context = new ShopOnlineDBEntities())
                {
                    products = context.Products.Where(p => (string.IsNullOrEmpty(productToSearch.ProductName) || p.ProductName == productToSearch.ProductName) &&
                        (string.IsNullOrEmpty(productToSearch.Brand) || p.Brand == productToSearch.Brand) &&
                        (productToSearch.Cost == 0 || p.Cost == productToSearch.Cost) &&
                        (productToSearch.Sector == 0 || p.Sector == productToSearch.Sector))
                        .ToList();
                }
            }
            else
            {
                using (var context = new ShopOnlineDBEntities())
                {
                    products = context.Products.Where(p => (productToSearch.ProductId == 0 || p.ProductId == productToSearch.ProductId))
                        .ToList();
                }
            }

            foreach (var p in products)
            {
                Product productToGet = new Product()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Brand = p.Brand,
                    Cost = p.Cost,
                    Quantity = p.Quantity,
                    Sectors = (Sector)p.Sector
                };
                productsToGet.Add(productToGet);
            }

            return productsToGet;
        }

    }
}
