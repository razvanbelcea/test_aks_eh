using static eathappy.order.domain.Article.ArticleEnums;

namespace eathappy.order.domain.Dtos.Local.Parameter
{
    public  class ArticleUpdateDto
    {
        public int Quantity { get; set; }
        public ReasonCode ReasonCode { get; set; }
        public ArticleStatus Status { get; set; }
    }
}
