using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.Infrastructure.Auth;
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
        services.AddQuartzCleanUpJob(configurationManager);
        
        /* Authorization and authentication */
        var authSettings = new AuthSettings();
        configurationManager.Bind(AuthSettings.SectionName, authSettings);
        services.AddSingleton(Options.Create(authSettings));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidIssuer = authSettings.Issuer,
                    ValidAudience = authSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true
                };
            });
        
        services.AddAuthorization(config =>
        {
            config.AddPolicy("owner-only", policy =>
                policy.Requirements.Add(new IsRegisteredRequirement()));
        });
        
        services.AddSingleton<IAuthorizationHandler, AuthHandler>();
        services.AddHttpContextAccessor();
        return services;
    }
}