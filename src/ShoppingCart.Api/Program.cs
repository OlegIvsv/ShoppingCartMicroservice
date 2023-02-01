using Hellang.Middleware.ProblemDetails;
using ShoppingCart.Api;
using ShoppingCart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApiPresentation(builder.Host);
    builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseProblemDetails();
    app.UseHttpLogging();

    var apiVersionDescriptionProvider = app.Services
        .GetRequiredService<IApiVersionDescriptionProvider>();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

namespace ShoppingCart.Api
{
    public partial class Program { }
}