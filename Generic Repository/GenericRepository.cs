using MVC_Assignment.Models;
using MVC_Assignment.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Generic_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> , IDisposable where T : class
    {
        private IDbSet<T> _dbSet;
        private string _errorMessage = string.Empty;
        private bool _disposed = false;
        public ShoppingCartContext Context { get; set; }

        public GenericRepository(IUnitOfWork<ShoppingCartContext> unitOfWork)
            : this(unitOfWork.Context)
        {
        }

        public GenericRepository(ShoppingCartContext context)
        {
            _disposed = false;
            Context = context;
            _dbSet = context.Set<T>();
        }
        protected virtual IDbSet<T> Entities
        {
            get { return _dbSet ?? (_dbSet = Context.Set<T>()); }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity");
                }
                if (Context == null || _disposed)
                {
                    Context = new ShoppingCartContext();
                }

                Entities.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _disposed = true;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity");
                }

                if (Context == null || _disposed)
                {
                    Context = new ShoppingCartContext();
                }
                Entities.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        private void HandleUnitOfWorkException(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
                }
            }
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity");
                }

                if (Context == null || _disposed)
                {
                    Context = new ShoppingCartContext();
                }
                Context.Entry(entity).State = EntityState.Modified;
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }
    }
}