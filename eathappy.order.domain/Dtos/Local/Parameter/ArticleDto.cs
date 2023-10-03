using System;

namespace eathappy.order.domain.Dtos.Local.Parameter
{
    public class ArticleDto
    {
        public long Sku { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
    }
}
