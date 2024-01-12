using Ddd.Example.Service.Api.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Api.Controllers.Filters.V10
{

    public class ApiActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<ApiActionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiActionFilter"/> class.
        /// </summary>
        /// <param name="logger"><see cref="ILogger{ApiActionFilter}"/>.</param>
        public ApiActionFilter(ILogger<ApiActionFilter> logger) =>
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public async Task OnActionExecutionAsync(ActionExecutingContext actionExecutingContext, ActionExecutionDelegate next)
        {
            await CheckHeaderRequestIdAsync(actionExecutingContext)
                     .ContinueWith(
                         async taskCheckHeaderRequestId =>
                         {
                             if (taskCheckHeaderRequestId.Status != TaskStatus.Canceled)
                             {
                                 await CheckModelStateAsync(actionExecutingContext);
                             }
                         });

            await next()
                    .ContinueWith(
                        async taskNext =>
                        {
                            if (taskNext.Status != TaskStatus.Canceled)
                            {
                                await CheckResultAsync(actionExecutingContext, await taskNext);
                            }
                        });
        }

        /// <summary>
        /// Check head of request
        /// </summary>
        /// <param name="actionExecutingContext"><see cref="ActionExecutingContext"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        private Task CheckHeaderRequestIdAsync(ActionExecutingContext actionExecutingContext)
        {
            if (!actionExecutingContext.HttpContext.Request.Headers.TryGetValue(ApiProblemDetails.HeaderRequestIdName, out _))
            {
                if (actionExecutingContext.HttpContext.Request.Headers.TryGetValue("Referer", out _))
                {
                    actionExecutingContext.HttpContext.Request.Headers[ApiProblemDetails.HeaderRequestIdName] = Guid.NewGuid().ToString();

                    return Task.CompletedTask;
                }
                else
                {
                    actionExecutingContext.HttpContext.Response.StatusCode = StatusCodes.Status412PreconditionFailed;

                    var apiProblemDetails = new ApiProblemDetails(
                            actionExecutingContext.HttpContext,
                            $"No Id {ApiProblemDetails.HeaderRequestIdName}");

                    var objectResult = new ObjectResult(apiProblemDetails);

                    objectResult.StatusCode = actionExecutingContext.HttpContext.Response.StatusCode;

                    actionExecutingContext.Result = objectResult;

                    _logger.LogError(
                        ApplicationLogEvent.EVENT_DDD_EXAMPLE_SERVICE_PRECONDITION_FAILED,
                        "Request not approved {@ApiProblemDetails}",
                        apiProblemDetails);

                    return Task.FromCanceled(new CancellationToken(true));
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Check models
        /// </summary>
        /// <param name="actionExecutingContext"><see cref="ActionExecutingContext"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        private Task CheckModelStateAsync(ActionExecutingContext actionExecutingContext)
        {
            if (!actionExecutingContext.ModelState.IsValid)
            {
                actionExecutingContext.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var apiProblemDetails = new ApiProblemDetails(actionExecutingContext.HttpContext, actionExecutingContext.ModelState);

                var objectResult = new ObjectResult(apiProblemDetails)
                {
                    StatusCode = actionExecutingContext.HttpContext.Response.StatusCode
                };

                actionExecutingContext.Result = objectResult;

                _logger.LogError(
                    ApplicationLogEvent.EVENT_DDD_EXAMPLE_SERVICE_BAD_REQUEST,
                    "Request not approve {@ApiProblemDetails}",
                    apiProblemDetails);

                return Task.FromCanceled(new CancellationToken(true));
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Check result.
        /// </summary>
        /// <param name="actionExecutingContext"><see cref="ActionExecutingContext"/>.</param>
        /// <param name="actionExecutedContext"><see cref="ActionExecutedContext"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        private Task CheckResultAsync(ActionExecutingContext actionExecutingContext, ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Result is NotFoundResult)
            {
                actionExecutingContext.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                var apiProblemDetails = new ApiProblemDetails(actionExecutingContext.HttpContext, "Information not found");

                var objectResult = new ObjectResult(apiProblemDetails)
                {
                    StatusCode = actionExecutingContext.HttpContext.Response.StatusCode
                };

                actionExecutedContext.Result = objectResult;

                _logger.LogError(
                    ApplicationLogEvent.EVENT_DDD_EXAMPLE_SERVICE_NOT_FOUND,
                    "Not find {@ApiProblemDetails}",
                    apiProblemDetails);

                return Task.FromCanceled(new CancellationToken(true));
            }

            return Task.CompletedTask;
        }
    }
}
