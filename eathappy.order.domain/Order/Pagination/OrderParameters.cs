using eathappy.order.domain.Pagination;
using System;

namespace eathappy.order.domain.Order.Pagination
{
    public class OrderParameters : QueryStringParameters
    {
        public string[] HubNames { get; set; }
        public string[] CustomerIds { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public OrderState? OrderState { get; set; }
        public bool ServedByStore { get; set; }
    }
}
