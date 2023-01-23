using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ShoppingCart.Infrastructure.DataAccess.MongoDb;
using ShoppingCart.Interfaces.Interfaces;

namespace ShoppingCart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddSingleton<IShoppingCartRepository, MongoShoppingCartRepository>();

        var mongoSettings = new MongoSettings();
        configurationManager.Bind(MongoSettings.SectionName, mongoSettings);
        services.AddSingleton(Options.Create(mongoSettings));

        return services;
    }
}