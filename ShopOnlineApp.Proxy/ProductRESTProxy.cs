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
    public class ProductRESTProxy : IProductProxy
    {
        public AddResult AddProduct(Product product)
        {
            HttpClient client = IniziatizeClient();
            var response = client.PostAsJsonAsync("Product/AddProduct", product).Result;
            var addResult = GetRequestResult<AddResult>(response);
            return addResult;
        }

        public DeleteResult DeleteProduct(Product product)
        {
            HttpClient client = IniziatizeClient();
            var response = client.PostAsJsonAsync("Product/DeleteProduct", product).Result;
            var deleteResult = GetRequestResult<DeleteResult>(response);
            return deleteResult;
        }

        public SearchResult SearchProducts(Product product)
        {
            HttpClient client = IniziatizeClient();
            var response = client.PostAsJsonAsync("Product/SearchProducts", product).Result;
            var searchResult = GetRequestResult<SearchResult>(response);
            return searchResult;
        }

        public UpdateResult UpdateProduct(Product oldProduct, Product newProduct)
        {
            UpdateProductDTO updateProduct = new UpdateProductDTO();
            updateProduct.OldProduct = oldProduct;
            updateProduct.NewProduct = newProduct;
            var client = IniziatizeClient();
            var response = client.PostAsJsonAsync("Product/UpdateProduct", updateProduct).Result;
            var updateResult = GetRequestResult<UpdateResult>(response);
            return updateResult;
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
