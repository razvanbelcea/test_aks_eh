using eathappy.order.domain.Types;
using System;
using static eathappy.order.domain.Article.ArticleEnums;

namespace eathappy.order.domain.Article
{
    public class Article : BaseEntity<Guid>
    {
        public const string DefaultSchema = "catalog";
        public long Sku { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public Guid ReplacementArticle { get; set; }
        public ArticleStatus Status { get; set; }
        public ReasonCode ReasonCode { get; set; }

        #region Navigation properties
        public virtual Guid OrderId { get; set; }
        public virtual Order.Order Order { get; set; }
        #endregion
    }
}
