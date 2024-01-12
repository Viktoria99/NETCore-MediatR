using Ddd.Example.Service.Api.Controllers.Filters.V10;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Api.Logging
{

    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiActionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> of <see cref="ApiActionFilter"/>.</param>
        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiActionFilter> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Next to Middleware
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (!string.IsNullOrWhiteSpace(exception.Message))
                {
                    _logger.LogError(
                           ApplicationLogEvent.EVENT_DDD_EXAMPLE_SERVICE_ERROR,
                           exception,
                           exception.Message,
                           new ApiProblemDetails(context, exception.Message));
                }

                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Handle error
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <param name="exception"><see cref="Exception"/>.</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiProblemDetails apiProblemDetails;
            switch (exception)
            {

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    apiProblemDetails = new ApiProblemDetails(context, exception);
                    break;
            }

            context.Response.ContentType = "application/json";
            var response = JsonConvert.SerializeObject(apiProblemDetails);

            return context.Response.WriteAsync(response);
        }
    }
}
