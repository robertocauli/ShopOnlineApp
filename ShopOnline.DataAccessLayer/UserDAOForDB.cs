using ShopOnline.DataAccessLayer.Model;
using ShopOnline.Model;
using System.Collections.Generic;
using System.Linq;

namespace ShopOnline.DataAccessLayer
{
    public class UserDAOForDB : IUserDAO
    {
        public void CreateUser(User user)
        {
            Users userToAdd = new Users()
            {
                Username = user.Username,
                Password = user.Password,
                RolesId = (int)user.Roles
            };

            using (var context = new ShopOnlineDBEntities())
            {
                context.Users.Add(userToAdd);
                context.SaveChanges();
            }
        }

        public List<User> GetUsers(User user)
        {
            List<User> users = new List<User>();
            List<Users> userList = new List<Users>();

            Users userToSearch = new Users
            {
                Username = user?.Username,
                Password = user?.Password,
                UserId = (int)(user?.UserId)
            };

            //if(string.IsNullOrEmpty(userToSearch.Password) && userToSearch.UserId == 0)
            //{
            //    using (var context = new ShopOnlineDBEntities())
            //    {
            //        userList = context.Users.Where(u => (string.IsNullOrEmpty(userToSearch.Username) || u.Username == userToSearch.Username)).ToList();
            //    }
            //}
            if (userToSearch.UserId == 0)
            {
                using (var context = new ShopOnlineDBEntities())
                {
                    userList = context.Users.Where(u => (string.IsNullOrEmpty(userToSearch.Username) || u.Username == userToSearch.Username)&&
                    (string.IsNullOrEmpty(userToSearch.Password) || u.Password == userToSearch.Password)).ToList();
                }
            }
            else
            {
                using (var context = new ShopOnlineDBEntities())
                {
                    userList = context.Users.Where(u => (string.IsNullOrEmpty(userToSearch.Username) || u.Username == userToSearch.Username) &&
                    (userToSearch.UserId == 0 || u.UserId == userToSearch.UserId))
                        .ToList();
                }
            }

            foreach (var u in userList)
            {
                User userInList = new User
                {
                    Username = u.Username,
                    Password = u.Password,
                    UserId = u.UserId,
                    Roles = (Role)u.RolesId
                };
                users.Add(userInList);
            }
            return users;
        }
    }
}
