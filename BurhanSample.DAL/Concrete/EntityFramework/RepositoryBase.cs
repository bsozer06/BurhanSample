using BurhanSample.DAL.Concrete.EntityFramework.Context;
using Core.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.DAL.Concrete.EntityFramework
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        protected RepositoryContext _context;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        // AsNoTracking: to improve our read-only query performance
        public IQueryable<T> GetAll(bool trackChanges)
        {
            if (!trackChanges)
                return _context.Set<T>().AsNoTracking();
            else
                return _context.Set<T>();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (!trackChanges)
                return _context.Set<T>().Where(expression).AsNoTracking();
            else
                return _context.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        
        public void Update(T entity) => _context.Set<T>().Update(entity);
        
    }
}
