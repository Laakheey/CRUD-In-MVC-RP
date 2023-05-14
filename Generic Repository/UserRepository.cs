using MVC_Assignment.Models;
using MVC_Assignment.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Generic_Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork<ShoppingCartContext> unitOfWork) : base(unitOfWork)
        {
        }
        public UserRepository(ShoppingCartContext context) : base(context)
        {
        }
        public User GetUserByEmail(string email)
        {
            return Context.Users.Where(u=>u.Email == email).FirstOrDefault();
        }

        public User GetUserByName(string name)
        {
            return Context.Users.Where(u => u.Name == name).FirstOrDefault();
        }
    }
}