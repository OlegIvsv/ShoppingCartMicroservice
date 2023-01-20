using System.Net.Http.Json;
using ShoppingCart.Api.Contracts;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;

public class RemoveItemFromCartTests : CartsControllerIntegrationTestsBase
{
    [Theory]
    [InlineData("dfba8243-47b2-4ea8-88ac-1ab436bb0a48")] // Id from test data
    [InlineData("3eba8c48-01a6-4383-bea7-3b2876d6e2d3")] // Random id
    public async Task RemoveItemFromCart_ItemExistOrNot_RemovesItemAndReturnsOk(string id)
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.First().Id;
        Guid productId = Guid.Parse(id);
        //Act
        var queryString = $"api/cart/remove-item/{cartId}?productId={productId}";
        HttpResponseMessage response = await _client.PutAsync(queryString, null);
        //Assert
        AssertOK(response);
        await AssertItemIsNotInDb(cartId, productId);
    }

    [Fact]
    public async Task RemoveItemFromCart_CartDoesNotExists_ReturnsNotFound()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/remove-item/{cartId}?productId={productId}", null);
        //Assert
        AssertNotFound(response);
        AssertJsonProblemUtf8(response);
    }
}