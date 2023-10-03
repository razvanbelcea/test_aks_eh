using eathappy.order.domain.Order;
using eathappy.order.domain.Order.Pagination;
using eathappy.order.domain.Pagination;
using System;
using System.Threading.Tasks;

namespace eathappy.order.data.Interfaces.Specific
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PagedList<Order>> GetAllStoreOrders(OrderParameters parameters);
        Task<PagedList<Order>> GetAllHubOrders(OrderParameters parameters);
        Task<Order> GetOrder(Guid? orderId);
    }
}
