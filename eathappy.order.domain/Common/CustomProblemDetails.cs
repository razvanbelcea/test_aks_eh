using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eathappy.order.domain.Common
{
    public class CustomProblemDetails : ProblemDetails
    {
        public string Code { get; set; }

        public Dictionary<string, IEnumerable<ErrorDetails>> Errors { get; set; }

        //public IEnumerable<string> MissingPermissions { get; set; }

        public object Data { get; set; }

        public object FormerRequest { get; set; }
    }

    public class CustomProblemDetails<T> : CustomProblemDetails
        where T : class
    {
        public new T Data { get; set; }
    }
}
