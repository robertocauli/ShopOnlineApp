using ShopOnline.Model;

namespace ShopOnlineApp.Proxy
{
    public interface IProductProxy
    {
        SearchResult SearchProducts(Product product);
        AddResult AddProduct(Product product);
        UpdateResult UpdateProduct(Product oldProduct, Product newProduct);
        DeleteResult DeleteProduct(Product product);
    }
}
