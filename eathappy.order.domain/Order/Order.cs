using eathappy.order.domain.Types;
using System;
using System.Collections.Generic;

namespace eathappy.order.domain.Order
{
    public class Order : BaseEntity<Guid>
    {
        public const string DefaultSchema = "catalog";
        public string HubName { get; set; }
        public string CustomerId { get; set; }
        public long EdiNumber { get; set; }
        public Guid PosId { get; set; }
        public Guid FlinkCodeId { get; set; }
        public string CostCenterId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderState State { get; set; }
        public string OrderFile { get; set; }
        public bool ServedByStore { get; set; }

        #region Navigation properties
        public ICollection<Article.Article> Articles { get; set; }
        #endregion
    }
}
