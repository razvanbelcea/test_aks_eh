using eathappy.order.data.Context;
using eathappy.order.data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eathappy.order.data.Extensions;
using eathappy.order.domain.Types;

namespace eathappy.order.data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly DatabaseContext _context;
        protected DbSet<T> _table = null;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<T> Get(object id)
            => await _table.FindAsync(id);

        public async Task<T> Find(Expression<Func<T, bool>> expression)
            => await _table.FirstOrDefaultAsync(expression);

        public async Task<IEnumerable<T>> GetAll()
            => await _table.ToListAsync();

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> expression)
            => await _table.Where(expression).ToListAsync();

        public virtual IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            return _table.IncludeProperties(includeProperties);
        }

        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> filter)
        {
            return _table.Filter(filter);
        }

        public void Add(T entity) 
            => _table.Add(entity);

        public void AddRange(IEnumerable<T> entities)
            => _table.AddRange(entities);

        public void Update(T entity)
            => _table.Update(entity);

        public void UpdateRange(IEnumerable<T> entities)
            => _table.UpdateRange(entities);

        public void Delete(T entity)
            => _table.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities)
            => _table.RemoveRange(entities);
    }
}
