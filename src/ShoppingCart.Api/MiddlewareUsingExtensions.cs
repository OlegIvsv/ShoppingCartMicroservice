using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ShoppingCart.Api;

public static class MiddlewareUsingExtensions
{
    public static WebApplication UseSwaggerUIInDev(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return app;
        
        var apiVersionDescriptionProvider = app.Services
            .GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
        return app;
    }
}