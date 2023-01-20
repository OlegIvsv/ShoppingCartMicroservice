using ShoppingCart.Api.Contracts.ContractBinders;

namespace ShoppingCart.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiPresentation(this IServiceCollection services)
    {
        services.AddControllers(options => { options.ModelBinderProviders.Insert(0, new CartItemBinderProvider()); });
        return services;
    }
}