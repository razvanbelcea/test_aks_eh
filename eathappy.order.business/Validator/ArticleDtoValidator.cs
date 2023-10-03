using eathappy.order.business.Validator.Extensions;
using eathappy.order.domain.Common;
using eathappy.order.domain.Dtos.Local.Parameter;
using FluentValidation;

namespace eathappy.order.business.Validator
{
    public class ArticleDtoValidator : AbstractValidator<ArticleDto>
    {
        public ArticleDtoValidator()
        {
            RuleFor(x => x.Sku)
                .NotNull()
                .NotEmpty()
                .WithGlobalErrorCode(ErrorConstants.ErrorCodes.InvalidArticleNumber)
                .WithGlobalMessage(ErrorConstants.ErrorDescription.InvalidArticleNumber);

            RuleFor(x => x.Quantity)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithErrorCode(ErrorConstants.ErrorCodes.NegativeQuantityNotAllowed)
                    .WithMessage(ErrorConstants.ErrorDescription.NegativeQuantityNotAllowed)
                .GreaterThan(0)
                    .WithErrorCode(ErrorConstants.ErrorCodes.NegativeQuantityNotAllowed)
                    .WithMessage(ErrorConstants.ErrorDescription.NegativeQuantityNotAllowed);
        }
    }
}
