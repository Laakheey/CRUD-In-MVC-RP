using MVC_Assignment.Models;
using MVC_Assignment.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Generic_Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ShoppingCartContext context) : base(context)
        {
        }
        public ProductRepository(IUnitOfWork<ShoppingCartContext> unitOfWork) : base(unitOfWork)
        {
        }
        public IEnumerable<Product> GetProductByCategory(string category)
        {
            Category categoryEnum = (Category)Enum.Parse(typeof(Category), category);
            return Context.Products.Where(x => x.Category == categoryEnum).ToList();
        }


    }
}