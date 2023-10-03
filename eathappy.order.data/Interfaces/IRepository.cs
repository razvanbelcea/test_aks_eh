using eathappy.order.domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eathappy.order.data.Interfaces
{
    public interface IRepository<T>
        where T : class, IBaseEntity
    {
        Task<T> Get(object id);
        Task<T> Find(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> expression);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> Filter(Expression<Func<T, bool>> filter);
    }
}
