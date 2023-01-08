using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;
using ShoppingCart.Infrastructure.DataAccess;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests;

public class CartsControllerIntegrationTests : IDisposable
{
    private readonly IMongoCollection<Cart> _cartCollection;
    private readonly MongoDbRunner _runner;
    private readonly HttpClient _client;

    public CartsControllerIntegrationTests()
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


    [Fact]
    public async Task GetShoppingCart_CartExists_ReturnsOkResultWithData()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        var response = await _client.GetAsync($"api/cart/{cartId}");
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        var cartFromResponse = await response.Content.ReadFromJsonAsync<CartResponse>();
        Assert.NotNull(cartFromResponse);
        Assert.Equal(cartId, cartFromResponse.CustomerId);
    }

    [Fact]
    public async Task GetShoppingCart_CartDoesNotExists_ReturnsNotFound()
    {
        //Arrange
        await PrepareDatabase();
        Guid randomId = Guid.NewGuid();
        //Act
        var response = await _client.GetAsync($"api/cart/{randomId}");
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task GetShoppingCart_EmptyIdValue_ReturnsOkResult()
    {
        //Arrange
        await PrepareDatabase();
        Guid emptyId = Guid.Empty;
        //Act
        var response = await _client.GetAsync($"api/cart/{emptyId}");
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }


    [Fact]
    public async Task CreateShoppingCart_CartAlreadyExists_ReturnsConflictResult()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        var response = await _client.PostAsync($"api/cart/{cartId}", null);
        //Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task CreateShoppingCart_CartDoesNotExistYet_ReturnsOkWithData()
    {
        //Arrange
        await PrepareDatabase();
        Guid cartId = Guid.NewGuid();
        //Act
        var response = await _client.PostAsync($"api/cart/{cartId}", null);
        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        var cartFromResponse = await response.Content.ReadFromJsonAsync<CartResponse>();
        Assert.NotNull(cartFromResponse);
        Assert.Equal(cartId, cartFromResponse.CustomerId);
    }

    [Fact]
    public async Task CreateShoppingCart_InvalidId_ReturnsBadRequestResult()
    {
        //Arrange
        await PrepareDatabase();
        //Act
        var response = await _client.PostAsync($"api/cart/{0}", null);
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }


    [Fact]
    public async Task DeleteShoppingCart_CartExists_ReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        var response = await _client.DeleteAsync($"api/cart/{cartId}");
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteShoppingCart_CartDoesNotExist_NotFound()
    {
        //Arrange
        await PrepareDatabase();
        Guid randomId = Guid.NewGuid();
        //Act
        var response = await _client.DeleteAsync($"api/cart/{randomId}");
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task DeleteShoppingCart_InvalidId_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        //Act
        var response = await _client.DeleteAsync($"api/cart/{Guid.Empty}");
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }


    [Fact]
    public async Task ClearShoppingCart_CartExists_ReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.First().Id;
        //Act
        var response = await _client.PutAsync($"api/cart/clear/{cartId}", null);
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var cart = await _cartCollection.Find(c => c.Id == cartId).FirstAsync();
        Assert.Equal(0, cart.Items.Count);
    }

    [Fact]
    public async Task ClearShoppingCart_CartDoesNotExist_NotFound()
    {
        //Arrange
        await PrepareDatabase();
        Guid randomId = Guid.NewGuid();
        //Act
        var response = await _client.PutAsync($"api/cart/clear/{randomId}", null);
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task ClearShoppingCart_InvalidId_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        //Act
        var response = await _client.PutAsync($"api/cart/clear/{Guid.Empty}", null);
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }


    private async Task<List<Cart>> PrepareDatabase()
    {
        var testCarts = new List<Cart>
        {
            Cart.Create(Guid.Parse("008927df-3b96-4707-ad84-4e689d634987")).Value,
            Cart.Create(Guid.Parse("35454152-4895-42d7-b887-f274deff210d")).Value,
            Cart.Create(Guid.Parse("a7e1c434-8fae-44a9-a6ce-251c29119fc2")).Value
        };

        testCarts[0].PutItem(CreateTestItem("Test Product 1", 10, 100.10m, 0.1));
        testCarts[0].PutItem(CreateTestItem("Test Product 2", 20, 200.20m, 0.2));

        await _cartCollection.InsertManyAsync(testCarts);
        return testCarts;
    }

    private CartItem CreateTestItem(string title, int quantity, decimal price, double discount)
    {
        return CartItem.Create(
               Guid.NewGuid(),
               ProductTitle.Create(title).Value,
               Quantity.Create(quantity).Value,
               Money.Create(price).Value,
               Discount.Create(discount).Value)
           .Value;
    }

    public void Dispose()
    {
        _runner.Dispose();
    }
}










