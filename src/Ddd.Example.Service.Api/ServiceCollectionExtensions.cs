using Ddd.Example.Service.Api.HealthCheck;
using Ddd.Example.Service.Api.Logging;
using Ddd.Example.Service.Domain.Clients.V10;
using Ddd.Example.Service.Domain.Users.V10;
using Ddd.Example.Service.Infrastructure.Configuration;
using Ddd.Example.Service.Infrastructure.Database;
using Ddd.Example.Service.Infrastructure.Database.Repositories.Users.V10;
using Ddd.Example.Service.Infrastructure.Services;
using Ddd.Example.Service.Infrastructure.Services.Clients.V10.REST;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Ddd.Example.Service.Api
{
    /// <summary>
    /// ServiceCollectionExtensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        private static string ApplicationProjectNameSpace => $"{nameof(Ddd)}.{nameof(Example)}.{nameof(Service)}.{nameof(Application)}";

        private static string InfrastructureProjectNameSpace => $"{nameof(Ddd)}.{nameof(Example)}.{nameof(Service)}.{nameof(Infrastructure)}";

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) => services
            .AddOptions(configuration)
            .AddVersioning()
            .AddAdditionalLogState()
            .AddDataSources(configuration)
            .AddValidators()
            .AddMvcActionFilters()
            .AddAllHealthChecks()
            .AddMediator()
            .AddHttpContextAccessor()
            .AddAuthentication(configuration);


        private static IServiceCollection AddDomainHandlers(this IServiceCollection services) => services
            .Scan(scan =>
            {
                scan
                    .FromAssemblies(Assembly.Load(ApplicationProjectNameSpace), Assembly.Load(InfrastructureProjectNameSpace))
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(IRequestHandler<>)))
                    .AsImplementedInterfaces()
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(IRequestHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });


        private static IServiceCollection AddPipelineBehavior(this IServiceCollection services) => services
            .Scan(scan =>
            {
                scan
                    .FromAssemblies(Assembly.Load(ApplicationProjectNameSpace))
                        .AddClasses(classes => classes
                        .AssignableTo(typeof(IPipelineBehavior<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });


        private static IServiceCollection AddOData(this IServiceCollection services) => services
            .Scan(scan =>
            {
                scan.FromAssemblies(Assembly.Load(InfrastructureProjectNameSpace))
                        .AddClasses(classes => classes
                        .AssignableTo(typeof(IQueryBuilder<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });

        private static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddScoped<IUserRepository, UserRepository>();

        private static IServiceCollection AddRestServices(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(EndpointsOptions));
            var endpointsOptions = section.Get<EndpointsOptions>();

            return services.AddHttpServices<IClientService, ClientService>(endpointsOptions.ClientRest);
        }

        private static IServiceCollection AddHttpServices<TClientInterface, TImplementation>(
            this IServiceCollection services,
            EndpointItemOptions option)
            where TClientInterface : class
            where TImplementation : class, TClientInterface
        {
            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            services.AddHttpClient<TClientInterface, TImplementation>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    InitBasicHttpClient(option, client, mediaType);
                });

            return services;
        }

        private static void InitBasicHttpClient(EndpointItemOptions options, HttpClient client, MediaTypeWithQualityHeaderValue mediaType)
        {
            client.BaseAddress = new Uri(options.Host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            var headerAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{options.UserName}:{options.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", headerAuth);
        }


        private static IServiceCollection AddDataSources(this IServiceCollection services, IConfiguration configuration) => services
            .AddDbContext<DataContext>(o => o.UseSqlServer(configuration.GetConnectionString("Data")))
            .AddRepositories()
            .AddRestServices(configuration)
            .AddOData();

        private static IServiceCollection AddValidators(this IServiceCollection services) => services
            .Scan(scan =>
            {
                scan
                    .FromAssemblies(typeof(Startup).Assembly)
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

        private static IServiceCollection AddMvcActionFilters(this IServiceCollection services) => services
            .Scan(scan =>
            {
                scan
                    .FromAssemblies(typeof(Startup).Assembly)
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(IAsyncActionFilter)))
                    .AsSelf()
                    .WithScopedLifetime();
            });

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration) => services
            .Configure<DatabaseDapperOptions>(configuration.GetSection("ConnectionStrings"))
            .Configure<EndpointsOptions>(configuration.GetSection(nameof(EndpointsOptions)))
            .Configure<CacheOptions>(configuration.GetSection(nameof(CacheOptions)));


        private static IServiceCollection AddMediator(this IServiceCollection services) => services
            .AddMediatR(typeof(Startup))
            .AddPipelineBehavior()
            .AddDomainHandlers();


        private static IServiceCollection AddAllHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddCustomHealthChecks();

            return services;
        }

        private static IServiceCollection AddVersioning(this IServiceCollection services) => services
            .AddApiVersioning()
            .AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = $"'v'V";
                o.DefaultApiVersionParameterDescription = "Версия API";
                o.SubstituteApiVersionInUrl = true;
            });
    }
}