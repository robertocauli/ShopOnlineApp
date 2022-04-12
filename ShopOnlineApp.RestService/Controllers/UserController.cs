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
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }

        private IUserDAO userDAO = new UserDAOForDB();
        private IProductDAO productDAO = new ProductDAOForDB();
        private IPurchaseDAO purchasenDAO = new PurchaseDAOForDB();

        [HttpPost]
        [Route("api/User/Login")]
        public LoginResult Login(User user)
        {
            LoginResult loginResult = new LoginResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO, purchasenDAO, productDAO);
            loginResult = businessLogic.Login(user);
            return loginResult;
        } 

        [HttpPost]
        [Route("api/User/Registration")]
        public RegistrationResult Registration(User user)
        {
            RegistrationResult registrationResult = new RegistrationResult();
            BusinessLogic businessLogic = new BusinessLogic(userDAO,purchasenDAO,productDAO);
            registrationResult = businessLogic.Registration(user);
            return registrationResult;
        }
    }
}
