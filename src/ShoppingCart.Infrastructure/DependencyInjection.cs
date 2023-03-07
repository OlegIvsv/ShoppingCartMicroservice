using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ShoppingCart.Infrastructure.BackgroundJobs;
using ShoppingCart.Infrastructure.DataAccess.MongoDb;
using ShoppingCart.Interfaces.Interfaces;

namespace ShoppingCart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        /* Repository and data access */
        services.AddSingleton<IShoppingCartRepository, MongoShoppingCartRepository>();
        /* Data access setting */
        var mongoSettings = new MongoSettings();
        configurationManager.Bind(MongoSettings.SectionName, mongoSettings);
        services.AddSingleton(Options.Create(mongoSettings));
        /* Quartz and jobs */
        services.AddQuartzJobs(configurationManager);

        return services;
    }
}