using MongoDB.Driver;
using ShoppingCart.Domain.Entities;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;

public class ClearShoppingCartTests : CartsControllerIntegrationTestsBase
{
    [Fact]
    public async Task ClearShoppingCart_CartExists_ReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.First().Id;
        //Act
        HttpResponseMessage response = await _client.PutAsync($"api/cart/clear/{cartId}", null);
        //Assert
        AssertOK(response);
        Cart? cart = await _cartCollection.Find(c => c.Id == cartId).FirstAsync();
        Assert.Equal(0, cart.Items.Count);
    }

    [Fact]
    public async Task ClearShoppingCart_CartDoesNotExist_NotFound()
    {
        //Arrange
        await PrepareDatabase();
        var randomId = Guid.NewGuid();
        //Act
        HttpResponseMessage response = await _client.PutAsync($"api/cart/clear/{randomId}", null);
        //Assert
        AssertNotFound(response);
        AssertJsonProblemUtf8(response);
    }

    [Fact]
    public async Task ClearShoppingCart_InvalidId_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        //Act
        HttpResponseMessage response = await _client.PutAsync($"api/cart/clear/{Guid.Empty}", null);
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }
}