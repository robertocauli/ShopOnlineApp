using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopOnline.Model;

namespace ShopOnline.DataAccessLayer
{
    public interface IUserDAO
    {
        List<User> GetUsers(User user);
        void CreateUser(User user);
    }
}
