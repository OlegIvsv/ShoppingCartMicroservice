using ShoppingCart.Api.Mapping;

namespace ShoppingCart.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiPresentation(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }
    }
}
