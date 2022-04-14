using ShopOnline.Model;

namespace ShopOnlineApp.Proxy
{
    public interface IPurchaseProxy
    {
        ShoppingResult Shopping(Product product, int idUser);
        ResultReturn ReturnProduct(Product product, int idUser);
        SearchPurchaseResult SearchPurchase(Product product, User user, bool returnOrNot);
    }
}
