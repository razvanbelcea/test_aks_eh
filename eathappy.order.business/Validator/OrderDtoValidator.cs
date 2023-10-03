using eathappy.order.business.Validator.Extensions;
using eathappy.order.domain.Common;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Order;
using FluentValidation;
using System;

namespace eathappy.order.business.Validator
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(x => x.HubName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .WithGlobalErrorCode(ErrorConstants.ErrorCodes.InvalidHubName)
                .WithGlobalMessage(ErrorConstants.ErrorDescription.InvalidHubName);

            RuleFor(x => x.State)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEqual(OrderState.Confirmed)
                    .WithErrorCode(ErrorConstants.ErrorCodes.InvalidOrderState)
                    .WithMessage(ErrorConstants.ErrorDescription.InvalidOrderState);

            RuleFor(x => x.OrderDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .NotEqual(DateTime.MinValue)
                .WithGlobalErrorCode(ErrorConstants.ErrorCodes.InvalidOrderDate)
                .WithGlobalMessage(ErrorConstants.ErrorDescription.InvalidOrderDate);

            RuleFor(x => x.DeliveryDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .NotEqual(DateTime.MinValue)
                .When(x => DateTime.Compare(x.OrderDate, x.DeliveryDate) > 0)
                    .WithErrorCode(ErrorConstants.ErrorDescription.DeliveryDateBeforeOrderDate)
                    .WithMessage(ErrorConstants.ErrorDescription.DeliveryDateBeforeOrderDate)
                .WithGlobalErrorCode(ErrorConstants.ErrorCodes.InvalidDeliveryDate)
                .WithGlobalMessage(ErrorConstants.ErrorDescription.InvalidDeliveryOrderDate);

        }
    }
}
