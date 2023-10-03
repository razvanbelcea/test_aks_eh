using eathappy.order.data.Context;
using eathappy.order.data.Extensions;
using eathappy.order.data.Interfaces.Specific;
using eathappy.order.domain.Order;
using eathappy.order.domain.Order.Pagination;
using eathappy.order.domain.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eathappy.order.data.Repositories.Specific
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<PagedList<Order>> GetAllStoreOrders(OrderParameters parameters)
        {
            return await PagedList<Order>.ToPagedList(FilterResults(parameters, servedByStore: true)
                .IncludeProperties(o => o.Articles)
                .OrderBy(o => o.Id), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Order>> GetAllHubOrders(OrderParameters parameters)
        {
            return await PagedList<Order>.ToPagedList(FilterResults(parameters, servedByStore: false)
                .IncludeProperties(o => o.Articles)
                .OrderBy(o => o.Id), parameters.PageNumber, parameters.PageSize);
        }

        private IQueryable<Order> FilterResults(OrderParameters parameters, bool servedByStore)
        {
            return _table
                            .Where(o => 
                                (parameters.HubNames == null || parameters.HubNames.Any(x => x == o.HubName)) &&
                                (parameters.CustomerIds == null || parameters.CustomerIds.Any(x => x == o.CustomerId)) &&
                                (parameters.OrderState == null || o.State == parameters.OrderState) &&
                                (parameters.OrderDate == null || o.OrderDate == parameters.OrderDate) &&
                                (parameters.DeliveryDate == null || o.OrderDate == parameters.DeliveryDate) &&
                                (o.ServedByStore == servedByStore));
        }

        public async Task<Order> GetOrder(Guid? orderId)
        {
            return await _table
                .Filter(filter => filter.Id == (Guid)orderId)
                .IncludeProperties(order => order.Articles)
                .SingleOrDefaultAsync();
        }
    }
}
