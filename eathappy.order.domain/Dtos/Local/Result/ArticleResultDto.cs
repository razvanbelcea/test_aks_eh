using eathappy.order.domain.Article;
using Newtonsoft.Json;
using System;
using static eathappy.order.domain.Article.ArticleEnums;

namespace eathappy.order.domain.Dtos.Local.Result
{
    public class ArticleResultDto
    {
        public Guid Id { get; set; } 
        public long Sku { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public ArticleStatus Status { get; set; }
        public ReasonCode ReasonCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ReplacementArticle { get; set; }
    }
}
