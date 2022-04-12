using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineApp.Proxy
{
    public class UserRESTProxy : IUserProxy
    {
        //public LoginResult Login(User user)
        //{
        //    var client = IniziatizeClient();
        //}
        public LoginResult Login(User user)
        {
            throw new NotImplementedException();
        }

        public RegistrationResult Registration(User user)
        {
            throw new NotImplementedException();
        }
    }
}
