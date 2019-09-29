using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstraction
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void Edit(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
    }
}