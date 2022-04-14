using Newtonsoft.Json;
using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineApp.Proxy
{
    public class PurchaseRESTProxy : IPurchaseProxy
    {
        public ResultReturn ReturnProduct(Product product, int idUser)
        {
            ReturnProductDTO returnProduct = new ReturnProductDTO();
            returnProduct.Product = product;
            returnProduct.IdUser = idUser;
            var client = IniziatizeClient();
            var response = client.PostAsJsonAsync("Purchase/ReturnProduct", returnProduct).Result;
            var resultReturn = GetRequestResult<ResultReturn>(response);
            return resultReturn;
        }

        public SearchPurchaseResult SearchPurchase(Product product, User user, bool returnOrNot)
        {
            SearchPurchaseDTO searchPurchase = new SearchPurchaseDTO();
            searchPurchase.Product = product;
            searchPurchase.User = user;
            searchPurchase.ReturnOrNot = returnOrNot;
            var client = IniziatizeClient();
            var response = client.PostAsJsonAsync("Purchase/ReturnProduct", searchPurchase).Result;
            var searchPurchaseResult = GetRequestResult<SearchPurchaseResult>(response);
            return searchPurchaseResult;
        }

        public ShoppingResult Shopping(Product product, int idUser)
        {
            ShoppingDTO shopping = new ShoppingDTO();
            shopping.Product = product;
            shopping.IdUser = idUser;
            var client = IniziatizeClient();
            var response = client.PostAsJsonAsync("Purchase/ReturnProduct", shopping).Result;
            var shoppingResult = GetRequestResult<ShoppingResult>(response);
            return shoppingResult;
        }

        private static T GetRequestResult<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var control = JsonConvert.DeserializeObject<T>(content);

                return control;
            }
            else
            {
                throw new WebException(((int)response.StatusCode).ToString());
            }
        }

        private HttpClient IniziatizeClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/ShopOnlineApp.RestService/api/");
            return client;
        }
    }
}
