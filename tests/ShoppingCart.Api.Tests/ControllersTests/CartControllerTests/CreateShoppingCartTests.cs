using System.Net.Http.Json;
using MongoDB.Driver;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Api.Tests.ControllersTests.Extensions;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;

public class CreateShoppingCartTests : CartsControllerIntegrationTestsBase
{
    [Fact]
    public async Task CreateShoppingCart_CartAlreadyExists_ReturnsConflictResult()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        HttpResponseMessage response = 
            await _client.PostAsync($"api/cart/{cartId}?isAnonymous=true", null);
        //Assert
        response.AssertCondlict();
        response.AssertJsonProblemUtf8();
    }

    [Fact]
    public async Task CreateShoppingCart_CartDoesNotExistYet_ReturnsOkWithData()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        //Act
        HttpResponseMessage response = 
            await _client.PostAsync($"api/cart/{cartId}?isAnonymous=true", null);
        //Assert
        response.AssertCreated();
        response.AssertJsonUtf8();
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
        HttpResponseMessage response = 
            await _client.PostAsync($"api/cart/{0}?isAnonymous=true", null);
        //Assert
        response.AssertNotFound();
        response.AssertJsonProblemUtf8();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CreateShoppingCart_AnonymousCart_ReturnsCreatedWithAnonymousCart(
        bool shouldCreateAnonymouse)
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        string uri = $"api/cart/{cartId}?isAnonymous={shouldCreateAnonymouse}";
        //Act
        HttpResponseMessage response = await _client.PostAsync(uri, null);
        //Assert
        response.AssertCreated();
        response.AssertJsonUtf8();
        var responseBody = await response.Content.ReadFromJsonAsync<CartResponse>();
        var cartInDb = await _cartCollection.Find(c => c.Id == cartId).FirstAsync();
        Assert.NotNull(responseBody);
        Assert.NotNull(cartInDb);
        Assert.Equal(shouldCreateAnonymouse, responseBody.IsAnonymous);
        Assert.Equal(shouldCreateAnonymouse, cartInDb.IsAnonymous);
    }
}