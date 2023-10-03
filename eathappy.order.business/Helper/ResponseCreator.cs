using eathappy.order.domain.Common;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace eathappy.order.business.Helper
{
    public static class ResponseCreator
    {
        public static Response<T> CreateValidResponse<T>(T dto)
        {
            return CreateResponse(dto, new ValidationResult());
        }
        public static Response<TDto> CreateValidationResultResponse<TDto>(TDto responseDto, ValidationResult validationResult)
        {
            return new Response<TDto>(responseDto)
            {
                IsValid = validationResult.IsValid,
                ValidationResult = validationResult
            };
        }

        public static Response<T> CreateResponse<T>(T dto, ValidationResult validationResult)
        {
            var response = new Response<T>(dto) { IsValid = validationResult.IsValid };

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
            response.Errors.AddRange(errorMessages);

            var error = validationResult.Errors.FirstOrDefault();

            response.ProblemDetails = new CustomProblemDetails
            {
                Errors = CreateErrorDictionary(validationResult),
                Code = error?.ErrorCode,
                Title = error?.ErrorMessage,
                Status = (int)HttpStatusCode.BadRequest
            };

            return response;
        }

        public static Response<T> CreateInvalidResponseWithErrors<T>(params string[] errors)
        {
            var response = CreateInvalidResponse<T>();
            response.Errors.AddRange(errors);
            return response;
        }

        public static Response<T> CreateInvalidResponse<T>()
        {
            return new Response<T>
            {
                IsValid = false
            };
        }

        private static Dictionary<string, IEnumerable<ErrorDetails>> CreateErrorDictionary(ValidationResult validationResult)
            => validationResult.Errors
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(grouping => grouping.Key,
                    grouping => grouping.Select(failure => new ErrorDetails
                    { Code = failure.ErrorCode, Description = failure.ErrorMessage }));
    }
}
