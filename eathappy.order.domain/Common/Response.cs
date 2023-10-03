using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eathappy.order.domain.Common
{
    public class Response
    {
        public bool IsValid { get; set; }

        public List<string> Errors { get; set; }

        public CustomProblemDetails ProblemDetails { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public ErrorDetails ErrorDetails { get; set; }

        public object Data { get; set; }

        public Response()
        {
            Errors = new List<string>();
            IsValid = true;
        }
    }

    public class Response<T> : Response
    {
        public new T Data { get; set; }

        public Response()
        {
        }

        public Response(T data)
        {
            Data = data;
        }
    }
}
