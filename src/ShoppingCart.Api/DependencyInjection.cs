using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using ShoppingCart.Api.Contracts.ContractBinders;
using ShoppingCart.Api.Middleware;
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
        services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new CartItemBinderProvider());
        });
       
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddTransient<ErrorHandlingMiddleware>();
        
        return services;
    }
}