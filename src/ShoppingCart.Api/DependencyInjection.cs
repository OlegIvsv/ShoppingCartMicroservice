using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using ShoppingCart.Api.Contracts.ContractBinders;
using ShoppingCart.Api.Contracts.RouteConstraints;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace ShoppingCart.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiPresentation(this IServiceCollection services, ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((hostingContext, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
        });
        /*  By default details is shown only in the Development environment */
        services.AddProblemDetails();
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All;
            
            /* Include versioning info in log messages */
            options.RequestHeaders.Add("x-api-version");
            options.ResponseHeaders.Add("api-supported-versions");
            options.ResponseHeaders.Add("api-deprecated-versions");

            options.RequestHeaders.Add("Authorization");
            
            options.RequestBodyLogLimit = 1024 * 8;
            options.ResponseBodyLogLimit = 1024 * 8;
        });
        services.AddRouting(options =>
        {
            options.ConstraintMap.Add("guidID", typeof(GuidIdConstraint));
        });
        services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new CartItemBinderProvider());
        });
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            options.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.ExampleFilters();
        });
        services.AddSwaggerExamplesFromAssemblyOf<Program>();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        
        return services;
    }
}

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
    }

    public void Configure(string name, SwaggerGenOptions options)
        => Configure(options);

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        var info = new OpenApiInfo()
        {
            Title = " ShoppingCart Microservice .NET 6 Web API",
            Version = desc.ApiVersion.ToString()
        };
        
        if (desc.IsDeprecated)
        {
            info.Description +=
                " This API version has been deprecated. Please use one of " +
                "the new APIs available from the explorer.";
        }
        
        return info;
    }
}
