using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> : IQuery<T> where T : class, new()
    {
        IQueryable<T> GetList(
    Expression<Func<T, bool>>? predicate = null,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
    bool enableTracking = true);
        public T Get(Expression<Func<T, bool>>? predicate,
    bool enableTracking = true);
        T Add(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
