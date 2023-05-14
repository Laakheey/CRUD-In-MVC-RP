using MVC_Assignment.Models;
using MVC_Assignment.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Generic_Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> GetProductByCategory(string category);
    }
}