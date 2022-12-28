using ShoppingCart.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ShoppingCart.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddScoped<IShoppingCartRepository, FakeShoppingCartRepository>();

            var mongoSettings = new MongoSettings();
            configurationManager.Bind(MongoSettings.SectionName, mongoSettings);
            services.AddSingleton(Options.Create(mongoSettings));

            return services;
        }
    }
}
