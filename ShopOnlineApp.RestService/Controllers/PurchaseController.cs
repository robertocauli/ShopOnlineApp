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
    public class PurchaseController : ApiController
    {
        // GET: api/Purchase
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Purchase/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Purchase
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Purchase/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Purchase/5
        public void Delete(int id)
        {
        }

        private IUserDAO userDAO = new UserDAOForDB();
        private IProductDAO productDAO = new ProductDAOForDB();
        private IPurchaseDAO purchasenDAO = new PurchaseDAOForDB();

        [HttpPost]
        [Route("api/Purchase/Shopping")]
        ShoppingResult Shopping(ShoppingDTO shoppingDTO)
        {
            ShoppingResult shoppingResult = new ShoppingResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO, purchasenDAO, productDAO);
            shoppingResult = businessLogic.Shopping(shoppingDTO.Product, shoppingDTO.IdUser);
            return shoppingResult;
        }

        [HttpPost]
        [Route("api/Purchase/ReturnProduct")]
        ResultReturn ReturnProduct(ReturnProductDTO returnProductDTO)
        {
            ResultReturn resultReturn = new ResultReturn();
            BusinessLogic businessLogic = new BusinessLogic(userDAO, purchasenDAO, productDAO);
            resultReturn = businessLogic.ReturnProduct(returnProductDTO.Product, returnProductDTO.IdUser);
            return resultReturn;
        }

        [HttpPost]
        [Route("api/Purchase/SearchPurchase")]
        SearchPurchaseResult SearchPurchase(SearchPurchaseDTO searchPurchaseDTO)
        {
            SearchPurchaseResult searchPurchaseResult = new SearchPurchaseResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO, purchasenDAO, productDAO);
            searchPurchaseResult = businessLogic.SearchPurchase(searchPurchaseDTO.Product, searchPurchaseDTO.User, searchPurchaseDTO.ReturnOrNot);
            return searchPurchaseResult;
        }
    }
}
