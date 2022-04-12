using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.DataAccessLayer
{
    public interface IProductDAO
    {
        void CreateProduct(Product product);
        List<Product> GetProducts(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int idProduct);
    }
}
