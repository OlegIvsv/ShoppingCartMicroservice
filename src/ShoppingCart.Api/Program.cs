using ShoppingCart.Api;
using ShoppingCart.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApiPresentation();
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
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

public partial class Program { }