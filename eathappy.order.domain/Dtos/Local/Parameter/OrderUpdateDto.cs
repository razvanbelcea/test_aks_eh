using eathappy.order.domain.Order;
using System;

namespace eathappy.order.domain.Dtos.Local.Parameter
{
    public class OrderUpdateDto
    {
        public DateTime DeliveryDate { get; set; }
        public OrderState State { get; set; }
    }
}
