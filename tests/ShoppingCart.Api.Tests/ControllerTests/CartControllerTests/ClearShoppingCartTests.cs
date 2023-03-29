using MongoDB.Driver;
using ShoppingCart.Api.Tests.Common;
using ShoppingCart.Api.Tests.ControllersTests.Extensions;
using ShoppingCart.Domain.Entities;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;

public class ClearShoppingCartTests : IntegrationTestBase
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
        response.AssertOK();
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
        response.AssertNotFound();
        response.AssertJsonProblemUtf8();
    }
}