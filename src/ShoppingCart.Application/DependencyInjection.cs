using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Services;

namespace ShoppingCart.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            return services;
        }
    }
}
