using ShoppingCart.Api;
using ShoppingCart.Api.Middleware;
using ShoppingCart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApiPresentation(builder.Host);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
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