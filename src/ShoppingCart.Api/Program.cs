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
    app.UseSwaggerUIInDev();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

namespace ShoppingCart.Api
{
    public partial class Program { }
}