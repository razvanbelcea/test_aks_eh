using AutoMapper;
using eathappy.order.business.Interfaces;
using eathappy.order.data.UnitOfWork;
using eathappy.order.domain.Article;
using eathappy.order.domain.Common;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Dtos.Local.Result;
using eathappy.order.domain.Order;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using static eathappy.order.business.Helper.ResponseCreator;
using System.Threading.Tasks;
using FluentValidation;
using static eathappy.order.domain.Article.ArticleEnums;

namespace eathappy.order.business.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<OrderService> _log;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ArticleDto> _articleDtoValidator;

        public ArticleService(
            ILogger<OrderService> log,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<ArticleDto> articleDtoValidator
            )
        {
            _log = log;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _articleDtoValidator = articleDtoValidator;
        }
        public async Task<Response<ArticleResultDto>> AddArticleToOrder(Guid? orderId, ArticleDto articleDto)
        {
            Precondition.IsNotNull(articleDto);

            var articleValidationResults = await _articleDtoValidator.ValidateAsync(articleDto);

            if (!articleValidationResults.IsValid)
                return CreateValidationResultResponse<ArticleResultDto>(null, articleValidationResults);

            var order = await _unitOfWork.OrderRepository.Find(o => o.Id == orderId);

            if (order == null) 
                return null;

            if (order.State == OrderState.Confirmed)
                return CreateInvalidResponseWithErrors<ArticleResultDto>(ErrorConstants.InvalidActionOnConfirmedOrder);

            var entity = _mapper.Map<Article>(articleDto);
            entity.OrderId = (Guid)orderId;
             _unitOfWork.ArticleRepository.Add(entity);

            return await _unitOfWork.SaveChangesAsync() ? CreateResponse(_mapper.Map<ArticleResultDto>(entity), articleValidationResults) : null;
        }

        public async Task<Response<ArticleResultDto>> AdjustQuantity(Guid? articleId, int quantity)
        {
            if (quantity <= 0)
                return CreateInvalidResponseWithErrors<ArticleResultDto>(ErrorConstants.InvalidArticleQuantity);

            var article = await _unitOfWork.ArticleRepository.Find(a => a.Id == articleId);

            if (article == null) 
                return null;

            article.Quantity = quantity;

            return await _unitOfWork.SaveChangesAsync() ? CreateValidResponse(_mapper.Map<ArticleResultDto>(article)) : null;
        }

        public async Task<IEnumerable<ArticleResultDto>> GetArticlesByOrderId(Guid? orderId)
        {
            var entities = await _unitOfWork.ArticleRepository.GetAll();

            var result = _mapper.Map<IEnumerable<ArticleResultDto>>(entities);

            return result;
        }

        public async Task<Response<ArticleResultDto>> ReplaceArticle(Guid? articleToReplace, ArticleDto replacementArticle)
        {
            Precondition.IsNotNull(replacementArticle);

            var articleValidationResults = await _articleDtoValidator.ValidateAsync(replacementArticle);

            if (!articleValidationResults.IsValid)
                return CreateValidationResultResponse<ArticleResultDto>(null, articleValidationResults);

            var oldArticle = await _unitOfWork.ArticleRepository.Find(a => a.Id == articleToReplace);

            if (oldArticle == null || oldArticle.Status == ArticleStatus.Voided) 
                return null;

            oldArticle.Status = ArticleStatus.Voided;

            var newArticle = _mapper.Map<Article>(replacementArticle);
            newArticle.OrderId = oldArticle.OrderId;
            _unitOfWork.ArticleRepository.Add(newArticle);
            oldArticle.ReplacementArticle = newArticle.Id;

            return await _unitOfWork.SaveChangesAsync() ? CreateValidResponse(_mapper.Map<ArticleResultDto>(newArticle)) : null;
        }

        public async Task<Response<ArticleResultDto>> UpdateArticle(Guid? articleId, ArticleUpdateDto articleUpdateDto)
        {
            var article = await _unitOfWork.ArticleRepository.Find(a => a.Id == articleId);

            if (article == null)
                return null;

            article.Status = articleUpdateDto.Status;
            article.ReasonCode = articleUpdateDto.ReasonCode;
            article.Quantity = articleUpdateDto.Quantity;

            return await _unitOfWork.SaveChangesAsync() ? CreateValidResponse(_mapper.Map<ArticleResultDto>(article)) : null;
        }

        public async Task<Response<ArticleResultDto>> UpdateArticleStatus(Guid? articleId, short status)
        {
            var article = await _unitOfWork.ArticleRepository.Find(a => a.Id == articleId);

            if (article == null)
                return null;

            article.Status = (ArticleStatus)status;

            return await _unitOfWork.SaveChangesAsync() ? CreateValidResponse(_mapper.Map<ArticleResultDto>(article)) : null;
        }
    }
}
