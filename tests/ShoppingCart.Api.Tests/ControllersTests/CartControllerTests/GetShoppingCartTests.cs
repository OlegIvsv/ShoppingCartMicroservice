using System.Net.Http.Json;
using ShoppingCart.Api.Contracts;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;

public class GetShoppingCartTests : CartsControllerIntegrationTestsBase
{
    [Fact]
    public async Task GetShoppingCart_CartExists_ReturnsOkResultWithData()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        HttpResponseMessage response = await _client.GetAsync($"api/cart/{cartId}");
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
        var randomId = Guid.NewGuid();
        //Act
        HttpResponseMessage response = await _client.GetAsync($"api/cart/{randomId}");
        //Assert
        AssertNotFound(response);
        AssertJsonProblemUtf8(response);
    }

    [Fact]
    public async Task GetShoppingCart_EmptyIdValue_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        var emptyId = Guid.Empty;
        //Act
        HttpResponseMessage response = await _client.GetAsync($"api/cart/{emptyId}");
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }
}