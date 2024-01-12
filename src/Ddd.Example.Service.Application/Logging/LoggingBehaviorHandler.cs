using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Application.Logging
{
    /// <inheritdoc />
    public class LoggingBehaviorHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<IPipelineBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehaviorHandler{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="logger"><see cref="ILogger{IPipelineBehavior{TRequest, TResponse}}"/>.</param>
        public LoggingBehaviorHandler(ILogger<IPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var operationName = typeof(TRequest).Name;

            try
            {
                _logger.LogInformation($"Operation {operationName}: {{@request_data}}", request);
                var response = await next();
                _logger.LogInformation($"Operation {operationName} success.Result: {{@response_data}}", response);
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error {operationName}");
                throw;
            }
        }
    }
}
