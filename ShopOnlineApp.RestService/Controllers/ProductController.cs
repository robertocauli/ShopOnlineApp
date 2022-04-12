using ShopOnline.BL;
using ShopOnline.DataAccessLayer;
using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopOnlineApp.RestService.Controllers
{
    public class ProductController : ApiController
    {
        // GET: api/Product
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Product/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Product
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Product/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
        }

        private IUserDAO userDAO = new UserDAOForDB();
        private IProductDAO productDAO = new ProductDAOForDB();
        private IPurchaseDAO purchasenDAO = new PurchaseDAOForDB();

        [HttpPost]
        [Route("api/Product/SearchProduct")]
        public SearchResult SearchProducts(Product product)
        {
            SearchResult searchResult = new SearchResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO,purchasenDAO,productDAO);
            searchResult = businessLogic.SearchProducts(product);
            return searchResult;
        }

        [HttpPost]
        [Route("api/Product/AddProduct")]
        public AddResult AddProduct(Product product)
        {
            AddResult addResult = new AddResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO,purchasenDAO,productDAO);
            addResult = businessLogic.AddProduct(product);
            return addResult;
        }

        [HttpPost]
        [Route("api/Product/UpdateProduct")]
        public UpdateResult UpdateProduct(UpdateProductDTO updateProduct)
        {
            UpdateResult updateResult = new UpdateResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO, purchasenDAO, productDAO);
            updateResult = businessLogic.UpdateProduct(updateProduct.OldProduct, updateProduct.NewProduct);
            return updateResult;
        }

        [HttpPost]
        [Route("api/Product/DeleteProduct")]
        DeleteResult DeleteProduct(Product product)
        {
            DeleteResult deleteResult = new DeleteResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO, purchasenDAO, productDAO);
            deleteResult = businessLogic.DeleteProduct(product);
            return deleteResult;
        }


    }
}
