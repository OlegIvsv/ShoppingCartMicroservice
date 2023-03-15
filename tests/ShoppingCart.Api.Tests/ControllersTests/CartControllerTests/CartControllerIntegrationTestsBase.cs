using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.ValueObjects;
using ShoppingCart.Infrastructure.DataAccess.MongoDb;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;

public class CartsControllerIntegrationTestsBase : IDisposable
{
    protected readonly IMongoCollection<Cart> _cartCollection;
    protected readonly MongoDbRunner _runner;
    protected readonly HttpClient _client;

    public CartsControllerIntegrationTestsBase()
    {
        _runner = MongoDbRunner.Start();

        var mongoSettings = new MongoSettings
        {
            ConnectionString = _runner.ConnectionString,
            Database = $"testdb_{DateTime.Now.Ticks}",
            ShoppingCartsCollection = $"testcoll_{DateTime.Now.Ticks}"
        };

        var appFactory = new WebApplicationFactory<Program>();
        appFactory = appFactory.WithWebHostBuilder(builder =>
        {
            // Disable Quartz clean-up job for tests
            builder.UseSetting("Jobs:CartCleanUpJob:Enabled", "false");
            // Setup services for testing
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(Options.Create(mongoSettings));
            });
            // Create an instance of repo to initialize serializators
            var mongoRepo = new MongoShoppingCartRepository(Options.Create(mongoSettings));
        });
        _client = appFactory.CreateClient();

        _cartCollection = new MongoClient(_runner.ConnectionString)
            .GetDatabase(mongoSettings.Database)
            .GetCollection<Cart>(mongoSettings.ShoppingCartsCollection);
    }

    protected async Task<List<Cart>> PrepareDatabase()
    {
        var testCarts = new List<Cart>
        {
            Cart.TryCreate(Guid.Parse("008927df-3b96-4707-ad84-4e689d634987"), true).Value,
            Cart.TryCreate(Guid.Parse("35454152-4895-42d7-b887-f274deff210d"), false).Value,
            Cart.TryCreate(Guid.Parse("a7e1c434-8fae-44a9-a6ce-251c29119fc2"), true).Value
        };

        testCarts[0].PutItem(
            CreateTestItem("dfba8243-47b2-4ea8-88ac-1ab436bb0a48", "Test Product 1", 10, 100.10m, 0.1));
        testCarts[0].PutItem(
            CreateTestItem("d7207775-ea71-4085-ba6a-482692b4aff8", "Test Product 2", 20, 200.20m, 0.2));

        await _cartCollection.InsertManyAsync(testCarts);
        return testCarts;
    }

    protected CartItem CreateTestItem(
        string id, 
        string title, 
        int quantity, 
        decimal price, 
        double discount,
        string imageUrl = "https://example.com/images/exaple.jpg")
    {
        return CartItem.TryCreate(
                Guid.Parse(id),
                ProductTitle.Create(title).Value,
                Quantity.Create(quantity).Value,
                Money.Create(price).Value,
                Discount.Create(discount).Value,
                ImageUrl.Create(imageUrl).Value)
            .Value;
    }

    protected object GetTestItemBody(
        Guid productId,
        decimal unitPrice,
        string productTitle,
        int itemQuantity,
        double discount,
        string imageUrl = "https://example.com/images/exaple.jpg")
    {
        return new
        {
            productId,
            unitPrice,
            productTitle,
            itemQuantity,
            discount,
            imageUrl
        };
    }
    
    protected async Task AssertItemIsInDb(Guid cartId, Guid productId, int expectedQuantity)
    {
        Cart? cart = await _cartCollection.Find(c => c.Id == cartId).FirstAsync();
        var result = cart.Items.Any(
            item => item.ProductId == productId
                    && item.ItemQuantity.Value == expectedQuantity);
        Assert.True(result, "Test database doesn't contain a product with these Id and Quantity");
    }

    protected async Task AssertItemIsNotInDb(Guid cartId, Guid productId)
    {
        Cart? cart = await _cartCollection.Find(c => c.Id == cartId).FirstAsync();
        var result = cart.Items.Any(item => item.ProductId == productId);
        Assert.False(result, "Test database contains a product with these Id");
    }
    
    public void Dispose()
    {
        _runner.Dispose();
    }
}