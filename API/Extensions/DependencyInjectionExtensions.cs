using System.Globalization;
using System.Text.Json.Serialization;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using TaskAggregator.API.ActionFilters.Async;
using TaskAggregator.API.Services;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Services;
using TaskAggregator.Infrastructure.Adapters.Application;
using TaskAggregator.Infrastructure.Adapters.Domain;
using TaskAggregator.Infrastructure.EfCore;
using TaskAggregator.Infrastructure.Options;
using TaskAggregator.Infrastructure.Options.Validators;

namespace TaskAggregator.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RequestSourceManagementDomainService>();
        services.AddScoped<RequestManagementDomainService>();
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediator(o =>
        {
            o.ServiceLifetime = ServiceLifetime.Transient;
            // o.PipelineBehaviors = [typeof(ValidationPipelineBehavior<,>)];
        });
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Options validators
        services.AddSingleton<IValidateOptions<DatabaseConnectionOptions>, DatabaseConnectionOptionsValidator>();

        // Options
        services.AddOptions<DatabaseConnectionOptions>()
            .BindConfiguration("DatabaseConnection").ValidateOnStart();

        // ETC
        services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
        {
            var dbOptions = serviceProvider
                .GetRequiredService<IOptions<DatabaseConnectionOptions>>()
                .Value;

            dbContextOptionsBuilder.UseNpgsql(dbOptions.ConnectionString);
        });

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IExcelReader, StreamExcelReader>();

        // Repositories
        services.AddScoped<IRequestSourceRepository, RequestSourceEfCoreRepository>();
        services.AddScoped<IRequestRepository, RequestEfCoreRepository>();

        // Query Svc
        services.AddScoped<IRequestSourceQueries, RequestSourceEfQueries>();
        services.AddScoped<IRequestQueries, RequestEfQueries>();

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        //serilog
        services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
        );

        services
            .AddMvc(
                o =>
                    {
                        // Учёт имени из JsonPropertyName атрибута, вместо имени свойства из DTO
                        o.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());

                        // auto fluent validation на данные методов контроллеров
                        o.Filters.Add<FluentValidationAsyncActionFilter>(order: -2001);
                    }
                )
            .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

        services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true; // ← Автоподстановка версии в URL
            }).AddOpenApi();

        services.Configure<RouteOptions>(setupAction =>
             {
                 setupAction.LowercaseUrls = true;
                 setupAction.LowercaseQueryStrings = true;
             });
        services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 3 * 1024 * 1024; // 3 MB BODY SIZE LIMIT 
            });

        services.AddSingleton<LocalizationService>();
        services.AddLocalization();
        services.AddRequestLocalization(o =>
        {
            o.SupportedCultures = o.SupportedUICultures = [new CultureInfo("en"), new CultureInfo("ru")];
            o.DefaultRequestCulture = new RequestCulture("en");
        });

        services.AddProblemDetails(opt => opt.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Type = null;
            ctx.ProblemDetails.Title = null;
            ctx.ProblemDetails.Extensions.Remove("traceId");
        });

        services.AddHealthChecks();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });
        services.AddHttpClient();
        // fluent validators
        services.AddValidatorsFromAssemblyContaining<TaskAggregator.API.Controllers.BaseController>(includeInternalTypes: true);
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
