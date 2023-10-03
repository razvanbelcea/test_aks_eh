using eathappy.order.domain.Common;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace eathappy.order.api.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ApiBaseController : ControllerBase
    {
        public ApiBaseController()
        {
        }

        protected IActionResult Result<T>(Response<T> response, object formerRequest = null)
        {
            if (response == null)
                return BadRequest();

            if (response.ValidationResult != null && !response.IsValid)
                return InvalidResponse(response.ValidationResult, formerRequest);

            //if (response.AuthorizationResponse != null && !response.IsValid)
            //    return ForbiddenResult(response.AuthorizationResponse, response.ErrorDetails, formerRequest);

            if (response.ProblemDetails != null && !response.IsValid)
                return BadRequestResult(response.ProblemDetails, formerRequest);

            return Ok(response.Data);
        }

        protected IActionResult InvalidResponse(ValidationResult validationResult, object formerRequest = null)
        {
            var error = validationResult.Errors.FirstOrDefault();

            var problemDetails = new CustomProblemDetails()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Code = error?.ErrorCode,
                Title = error?.ErrorMessage,
                Errors = WithErrors(validationResult),
                FormerRequest = formerRequest
            };

            return StatusCode((int)HttpStatusCode.BadRequest, problemDetails);
        }

        protected IActionResult BadRequestResult(CustomProblemDetails problemDetails, object formerRequest = null)
        {
            problemDetails.FormerRequest = null;

            return StatusCode((int)HttpStatusCode.BadRequest, problemDetails);
        }

        //protected IActionResult ForbiddenResult(AuthorizationResponse authorizationResponse, ErrorDetails errorDetails, object formerRequest = null)
        //{
        //    var problemDetails = ProblemDetailsBuilder
        //        .WithStatusCode(HttpStatusCode.Forbidden)
        //        .WithCode(errorDetails?.Code)
        //        .WithTitle(errorDetails?.Description)
        //        .WithAuthorization(authorizationResponse)
        //        .WithFormerRequest(formerRequest)
        //        .Build();

        //    return StatusCode((int)HttpStatusCode.Forbidden, problemDetails);
        //}

        private static Dictionary<string, IEnumerable<ErrorDetails>> WithErrors(ValidationResult validationResult)
        {
            return validationResult.Errors
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(
                    grouping => grouping.Key,
                    grouping => grouping.Select(failure => new ErrorDetails
                    { Code = failure.ErrorCode, Description = failure.ErrorMessage }));
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
