using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Ddd.Example.Service.Api.Logging
{

    public static class LoggingExtensions
    {
        /// <summary>
        /// Use log
        /// </summary>
        /// <param name="webHostBuilder"><see cref="IWebHostBuilder"/>.</param>
        /// <returns>IWebHostBuilder.</returns>
        public static IWebHostBuilder UseLogging(this IWebHostBuilder webHostBuilder) =>
            ServiceEnvironments.IsLocal() ? webHostBuilder.UseConsoleLogging() : webHostBuilder.UseTcpLogging();

        /// <summary>
        /// Add additional
        /// </summary>
        /// <param name="services"></param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddAdditionalLogState(this IServiceCollection services) => services
            .AddTransient<ILogState, AdditionalLogState>();
    }
}
