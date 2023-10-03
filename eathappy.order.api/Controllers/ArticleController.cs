using eathappy.order.business.Interfaces;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Dtos.Local.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace eathappy.order.api.Controllers
{
    /// <summary>
    /// Article controller
    /// </summary>
    [ApiController]
    [Route("orders/{orderId:guid}/articles")]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleService _articleService;

        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="articleService"></param>
        public ArticleController(
            ILogger<ArticleController> logger,
            IArticleService articleService
            )
        {
            _logger = logger;
            _articleService = articleService;
        }
        /// <summary>
        /// This method will get all the articles from a given order
        /// </summary>
        /// <returns>A list of article results</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<ArticleResultDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(Guid? orderId)
        {
            try
            {
                if (!orderId.HasValue)
                    return BadRequest();

                var result = await _articleService.GetArticlesByOrderId(orderId);

                return result == null ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@get_all_articles_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// This method will add a new article into the order
        /// </summary>
        /// <param name="orderId">The parent order id</param>
        /// <param name="article">Article line object (check Models section for the details)</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ArticleResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateArticleLine(Guid? orderId, [FromBody] ArticleDto article)
        {
            try
            {
                if (!orderId.HasValue)
                    return BadRequest();

                var response = await _articleService.AddArticleToOrder(orderId, article);

                var statusCode = response?.ProblemDetails?.Status ?? (int)HttpStatusCode.BadRequest;
                return response == null || !response.IsValid ? (IActionResult)StatusCode(statusCode, response) : Ok(response.Data);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@create_article_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// This method will update the quantity of the article
        /// </summary>
        /// <param name="articleId">The article id</param>
        /// <param name="quantity">New quantity</param>
        /// <returns></returns>
        [HttpPatch("{articleId}/adjustQuantity/{quantity}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ArticleResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdjustArticleQuantity(Guid? articleId, int quantity)
        {
            try
            {
                if (!articleId.HasValue)
                    return BadRequest();

                var response = await _articleService.AdjustQuantity(articleId, quantity);

                var statusCode = response?.ProblemDetails?.Status ?? (int)HttpStatusCode.BadRequest;
                return response == null || !response.IsValid ? (IActionResult)StatusCode(statusCode, response) : Ok(response.Data);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@adjust_article_quantity_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// This method will update the quantity of the article
        /// </summary>
        /// <param name="articleId">The article id</param>
        /// <param name="status">New quantity</param>
        /// <returns></returns>
        [HttpPatch("{articleId}/updateStatus/{status}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ArticleResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateArticleStatus(Guid? articleId, short status)
        {
            try
            {
                if (!articleId.HasValue)
                    return BadRequest();

                var response = await _articleService.UpdateArticleStatus(articleId, status);

                var statusCode = response?.ProblemDetails?.Status ?? (int)HttpStatusCode.BadRequest;
                return response == null || !response.IsValid ? (IActionResult)StatusCode(statusCode, response) : Ok(response.Data);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@adjust_article_quantity_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// This method will update the delivery date of the article
        /// </summary>
        /// <param name="articleId">The article id</param>
        /// <param name="replacementArticle">Replacement article</param>
        /// <returns></returns>
        [HttpPatch("{articleId}/replaceArticle")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ArticleResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReplaceArticle(Guid? articleId, ArticleDto replacementArticle)
        {
            try
            {
                if (!articleId.HasValue)
                    return BadRequest();

                var response = await _articleService.ReplaceArticle(articleId, replacementArticle);

                var statusCode = response?.ProblemDetails?.Status ?? (int)HttpStatusCode.BadRequest;
                return response == null || !response.IsValid ? (IActionResult)StatusCode(statusCode, response) : Ok(response.Data);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@replace_article_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Update article
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="articleUpdateDto"></param>
        /// <returns></returns>
        [HttpPatch("articleId:guid")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ArticleResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UopdateArticle(Guid? articleId, [FromBody] ArticleUpdateDto articleUpdateDto)
        {
            try
            {
                if (articleId.HasValue == false)
                    return BadRequest();

                var result = await _articleService.UpdateArticle(articleId, articleUpdateDto);

                return (result == null) ?
                    BadRequest() :
                    Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@update_article_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
