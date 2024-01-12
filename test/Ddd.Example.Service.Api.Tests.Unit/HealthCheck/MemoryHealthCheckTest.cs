using Ddd.Example.Service.Api;
using Ddd.Example.Service.Api.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Ddd.Example.Service.Tests.Unit.Api.HealthCheck
{

    public class MemoryHealthCheckTest
    {
        /// <summary>
        /// Add health checks configuration
        /// </summary>
        [Fact]
        public void AddHealthCheck_Properly_Configured()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddHealthChecks()
                .AddCheck<MemoryHealthCheck>(
                    "memory-check",
                    HealthStatus.Degraded,
                    new[] { "memory" });

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptionsSnapshot<HealthCheckServiceOptions>>();
            var registration = options.Value.Registrations.First();

            // Assert
            Assert.Equal("memory-check", registration.Name);
        }

        /// <summary>
        /// Test for working health check
        /// </summary>
        /// <returns>Task </returns>
        [Fact]
        public async Task WebHostBuilder_InitialStateMemoryHealthCheck_Healthy()
        {
            // Arrange
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(
                    services =>
                    {
                        services
                            .AddHealthChecks()
                            .AddCheck<MemoryHealthCheck>(
                                "memory-count",
                                HealthStatus.Degraded,
                                new[] { "memory" });
                    })
                .Configure(
                    app =>
                    {
                        app.UseHealthChecks(
                            "/health",
                            new HealthCheckOptions
                            {
                                Predicate = r => r.Tags.Contains("cards"),
                            });
                    });

            var server = new TestServer(webHostBuilder);

            // Act
            var response = await server.CreateRequest("/health")
                .GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}