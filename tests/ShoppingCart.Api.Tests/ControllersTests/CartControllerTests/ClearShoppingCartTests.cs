using MongoDB.Driver;
using ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests;

public class ClearShoppingCartTests : CartsControllerIntegrationTestsBase
{

    [Fact]
    public async Task ClearShoppingCart_CartExists_ReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.First().Id;
        //Act
        var response = await _client.PutAsync($"api/cart/clear/{cartId}", null);
        //Assert
        AssertOK(response);
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
        AssertNotFound(response);
        AssertJsonProblemUtf8(response);
    }

    [Fact]
    public async Task ClearShoppingCart_InvalidId_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        //Act
        var response = await _client.PutAsync($"api/cart/clear/{Guid.Empty}", null);
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }
}










