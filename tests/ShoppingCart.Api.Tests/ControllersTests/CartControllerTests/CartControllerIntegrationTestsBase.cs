using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;
using ShoppingCart.Infrastructure.DataAccess;
using System.Net;
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
        string databaseName = $"testdb_{new Random().Next()}";
        string collectionName = $"testcoll_{new Random().Next()}";


        var settings = new MongoSettings()
        {
            ConnectionString = _runner.ConnectionString,
            Database = databaseName,
            ShoppingCartsCollection = collectionName
        };
        var appFactory = new WebApplicationFactory<Program>();
        appFactory = appFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(Options.Create(settings));
            });
            // Create an instance of repo to initialize serializators
            //TODO: find another way to initialize serializators before filling test DB with data
            var mongoRepo = new MongoShoppingCartRepository(Options.Create(settings));
        });
        _client = appFactory.CreateClient();


        _cartCollection = new MongoClient(_runner.ConnectionString)
               .GetDatabase(databaseName)
               .GetCollection<Cart>(collectionName);
    }

    protected async Task<List<Cart>> PrepareDatabase()
    {
        var testCarts = new List<Cart>
        {
            Cart.Create(Guid.Parse("008927df-3b96-4707-ad84-4e689d634987")).Value,
            Cart.Create(Guid.Parse("35454152-4895-42d7-b887-f274deff210d")).Value,
            Cart.Create(Guid.Parse("a7e1c434-8fae-44a9-a6ce-251c29119fc2")).Value
        };

        testCarts[0].PutItem(
            CreateTestItem("dfba8243-47b2-4ea8-88ac-1ab436bb0a48", "Test Product 1", 10, 100.10m, 0.1));
        testCarts[0].PutItem(
            CreateTestItem("d7207775-ea71-4085-ba6a-482692b4aff8", "Test Product 2", 20, 200.20m, 0.2));

        await _cartCollection.InsertManyAsync(testCarts);
        return testCarts;
    }

    protected CartItem CreateTestItem(string id, string title, int quantity, decimal price, double discount)
    {
        return CartItem.Create(
               Guid.Parse(id),
               ProductTitle.Create(title).Value,
               Quantity.Create(quantity).Value,
               Money.Create(price).Value,
               Discount.Create(discount).Value)
           .Value;
    }

    protected async Task AssertItemIsInDb(Guid cartId, Guid productId, int expectedQuantity)
    {
        var cart = await _cartCollection.Find(c => c.Id == cartId).FirstAsync();
        bool result = cart.Items.Any(
            item => item.ProductId == productId
            && item.ItemQuantity.Value == expectedQuantity);
        Assert.True(result, "Test database doesn't contain a product with these Id and Quantity");
    }

    protected async Task AssertItemIsNotInDb(Guid cartId, Guid productId)
    {
        var cart = await _cartCollection.Find(c => c.Id == cartId).FirstAsync();
        bool result = cart.Items.Any(item => item.ProductId == productId);
        Assert.False(result, "Test database contains a product with these Id");
    }

    protected void AssertBadRequest(HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
    }

    protected void AssertNotFound(HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
    }

    protected void AssertCondlict(HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.Conflict, responseMessage.StatusCode);
    }

    protected void AssertCreated(HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
    }

    protected void AssertOK(HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    protected void AssertJsonUtf8(HttpResponseMessage responseMessage)
    {
        Assert.Equal(
           "application/json; charset=utf-8",
           responseMessage.Content.Headers.ContentType.ToString());
    }

    protected void AssertJsonProblemUtf8(HttpResponseMessage responseMessage)
    {
        Assert.Equal(
           "application/problem+json; charset=utf-8",
           responseMessage.Content.Headers.ContentType.ToString());
    }

    public void Dispose() => _runner.Dispose();
}










