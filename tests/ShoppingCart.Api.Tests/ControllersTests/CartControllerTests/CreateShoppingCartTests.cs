using System.Net.Http.Json;
using ShoppingCart.Api.Contracts;
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
        HttpResponseMessage response = await _client.PostAsync($"api/cart/{cartId}", null);
        //Assert
        AssertCondlict(response);
        AssertJsonProblemUtf8(response);
    }

    [Fact]
    public async Task CreateShoppingCart_CartDoesNotExistYet_ReturnsOkWithData()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        //Act
        HttpResponseMessage response = await _client.PostAsync($"api/cart/{cartId}", null);
        //Assert
        AssertCreated(response);
        AssertJsonUtf8(response);
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
        HttpResponseMessage response = await _client.PostAsync($"api/cart/{0}", null);
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }
}