using eathappy.order.domain.Order;
using System;

namespace eathappy.order.domain.Dtos.Local.Parameter
{
    public class OrderDto
    {
        public string HubName { get; set; }
        public string CustomerId { get; set; }
        public long EdiNumber { get; set; }
        public Guid PosId { get; set; }
        public Guid FlinkCodeId { get; set; }
        public string CostCenterId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderState State { get; set; }
        public bool ServedByStore { get; set; }
    }
}
