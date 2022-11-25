using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Data.IRepository
{
    public  interface IRepository<T> where T : class
    {
        List<T> GetAll();
        void Add(T item);
        void Delete(T item);

        void DeleteRange(IEnumerable<T> items);
        T FirstOrDefault(Expression<Func<T,bool>> filter);
    }
}
