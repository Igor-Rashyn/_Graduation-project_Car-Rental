using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using CarRental.DataContexts;
using CarRental.Models;

namespace CarRental.Repositories
{
    public class GenericRepository <T> : IRepository<T> where T: BaseEntity // <TType, TContext> : IRepository<TType> where TContext : ObjectContext, new() where TType : EntityObject
    {
        private AppDb _context;
        private DbSet<T> _dbSet;
      

        public DbSet<T> dbSet
        {
            get {
                if(_dbSet==null)
                {
                    _dbSet = _context.Set<T>();
                }
                return _dbSet;
            }
        }

        public GenericRepository(AppDb context)
        {
            this._context = context;
        }

        public void Create(T item)
        {
            try
            {
                if(dbSet==null)
                {
                    throw new ArgumentException("dbSet");
                }
                this.dbSet.Add(item);
              //  this._context.SaveChanges();
            }
            catch(DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Delete(T id)
        {
            try
            {
                if (dbSet == null)
                {
                    throw new ArgumentException("dbSet");
                }
                this.dbSet.Remove(id);
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                var fail = new Exception(msg, dbEx);
                throw fail;

            }
        }

        public T Find(object id)
        {
            return this.dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return this.dbSet;
        }

        public void Save()
        {
            this._context.SaveChanges();
        }

        public void Update(T item)
        {
            try
            {
                if (dbSet == null)
                {
                    throw new ArgumentException("dbSet");
                }
                _context.Entry(item).State = EntityState.Modified;
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
    }
}