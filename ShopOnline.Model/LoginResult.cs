using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class LoginResult
    {
        public List<User> UserLogged { get; set; }
        public bool IsLogged { get; set; }
    }
}
