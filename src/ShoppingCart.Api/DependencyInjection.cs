using Serilog;
using ShoppingCart.Api.Contracts.ContractBinders;
using ShoppingCart.Api.Middleware;

namespace ShoppingCart.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiPresentation(this IServiceCollection services, ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((hostingContext, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
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