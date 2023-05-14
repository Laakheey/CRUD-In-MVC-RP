using MVC_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Generic_Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetUserByName(string name);
        User GetUserByEmail(string email);

    }
}