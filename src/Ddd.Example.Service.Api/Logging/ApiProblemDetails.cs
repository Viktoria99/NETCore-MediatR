using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ddd.Example.Service.Api.Logging
{

    public class ApiProblemDetails : ProblemDetails
    {

        public const string HeaderRequestIdName = "REQUEST-ID";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiProblemDetails"/> class.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <param name="modelState"><see cref="ModelStateDictionary"/>.</param>
        public ApiProblemDetails(HttpContext context, ModelStateDictionary modelState)
            : this(context, "")
        {
            Errors = modelState
                .Where(w => !string.IsNullOrEmpty(w.Key))
                .ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.Errors.Select(n => !string.IsNullOrEmpty(n.ErrorMessage) ? n.ErrorMessage : n.Exception.Message));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiProblemDetails"/> class.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <param name="exception"><see cref="Exception"/>.</param>
        public ApiProblemDetails(HttpContext context, Exception exception)
              : this(context, exception.Message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiProblemDetails"/> class.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <param name="title"><see cref="string"/>.</param>
        public ApiProblemDetails(HttpContext context, string title)
        {
            Title = title;
            Status = context.Response.StatusCode;
            TraceId = context.Request.Headers[HeaderRequestIdName];
        }


        public string TraceId { get; }


        public IDictionary<string, IEnumerable<string>> Errors { get; }
    }
}
