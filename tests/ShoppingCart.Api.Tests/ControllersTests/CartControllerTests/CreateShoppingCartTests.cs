using Microsoft.AspNetCore.Http;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;
using System.Net.Http.Json;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests;

public class CreateShoppingCartTests : CartsControllerIntegrationTestsBase
{

    [Fact]
    public async Task CreateShoppingCart_CartAlreadyExists_ReturnsConflictResult()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        var response = await _client.PostAsync($"api/cart/{cartId}", null);
        //Assert
        AssertCondlict(response);
        AssertJsonProblemUtf8(response);
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
        var response = await _client.PostAsync($"api/cart/{0}", null);
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }
}










