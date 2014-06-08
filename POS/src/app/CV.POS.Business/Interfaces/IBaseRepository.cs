using System;
using System.Linq;
using System.Linq.Expressions;

namespace CV.POS.Business.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);

        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
