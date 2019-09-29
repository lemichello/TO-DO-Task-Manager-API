using System;
using System.Linq;
using System.Linq.Expressions;
using DAL.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementation
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        public EfRepository(EfContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void Edit(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }

        private readonly EfContext _context;
    }
}