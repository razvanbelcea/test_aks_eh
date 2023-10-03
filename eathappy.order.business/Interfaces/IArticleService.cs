using eathappy.order.domain.Common;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Dtos.Local.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eathappy.order.business.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleResultDto>> GetArticlesByOrderId(Guid? orderId);
        Task<Response<ArticleResultDto>> AddArticleToOrder(Guid? orderId, ArticleDto articleDto);
        Task<Response<ArticleResultDto>> AdjustQuantity(Guid? articleId, int quantity);
        Task<Response<ArticleResultDto>> ReplaceArticle(Guid? articleToReplace, ArticleDto replacementArticle);
        Task<Response<ArticleResultDto>> UpdateArticleStatus(Guid? articleId, short status);
        Task<Response<ArticleResultDto>> UpdateArticle(Guid? articleId, ArticleUpdateDto articleUpdateDto);
    }
}
