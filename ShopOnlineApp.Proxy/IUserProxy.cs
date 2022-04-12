using ShopOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineApp.Proxy
{
    public interface IUserProxy
    {
        LoginResult Login(User user);
        RegistrationResult Registration(User user);
    }
}
