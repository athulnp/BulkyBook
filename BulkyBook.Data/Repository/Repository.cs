using BulkyBook.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void Add(T item)
        {
            dbSet.Add(item);
        }

        public void Delete(T item)
        {
            dbSet.Remove(item);
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public List<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }
    }
}
