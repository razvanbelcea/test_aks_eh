using eathappy.order.domain.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace eathappy.order.data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, Expression<Func<T, bool>> filter)
            where T : IBaseEntity
        {
            return filter != null ? query.Where(filter) : query;
        }

        public static IQueryable<TEntity> IncludeProperties<TEntity>(
            this IQueryable<TEntity> query,
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class, IBaseEntity
        {
            return includeProperties != null ? includeProperties.Aggregate(query, (current, property) => current.Include(property)) : query;
        }
    }
}
