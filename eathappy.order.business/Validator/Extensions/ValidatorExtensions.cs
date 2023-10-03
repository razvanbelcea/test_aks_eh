using FluentValidation;
using FluentValidation.Internal;

namespace eathappy.order.business.Validator.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithGlobalMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorMessage)
        {
            var rules = rule as RuleBuilder<T, TProperty>;
            foreach (var item in rules.Rule.Validators)
            {
                item.Options.SetErrorMessage(errorMessage);
            }

            return rule;
        }

        public static IRuleBuilderOptions<T, TProperty> WithGlobalErrorCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorCode)
        {
            var rules = rule as RuleBuilder<T, TProperty>;
            foreach (var item in rules.Rule.Validators)
            {
                item.Options.ErrorCode = errorCode;
            }

            return rule;
        }
    }
}
