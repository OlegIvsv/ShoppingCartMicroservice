using Microsoft.AspNetCore.Http;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;
using System.Net.Http.Json;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests;

public class GetShoppingCartTests : CartsControllerIntegrationTestsBase
{

    [Fact]
    public async Task GetShoppingCart_CartExists_ReturnsOkResultWithData()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        var response = await _client.GetAsync($"api/cart/{cartId}");
        //Assert
        AssertOK(response);
        AssertJsonUtf8(response);
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
        AssertNotFound(response);
        AssertJsonProblemUtf8(response);
    }

    [Fact]
    public async Task GetShoppingCart_EmptyIdValue_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        Guid emptyId = Guid.Empty;
        //Act
        var response = await _client.GetAsync($"api/cart/{emptyId}");
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }
}










