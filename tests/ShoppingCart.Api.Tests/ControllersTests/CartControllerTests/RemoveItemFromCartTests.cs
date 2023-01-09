using MongoDB.Driver;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;
using System.Net.Http.Json;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests;

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
        string queryString = $"api/cart/remove-item/{cartId}?productId={productId}";
        var response = await _client.PutAsync(queryString, null);
        //Assert
        AssertOK(response); 
        await AssertItemIsNotInDb(cartId, productId);
    }

    [Fact]
    public async Task RemoveItemFromCart_CartDoesNotExists_ReturnsNotFound()
    {
        //Arrange
        await PrepareDatabase();
        Guid cartId = Guid.NewGuid();
        CartItemRequest bodyObject = new(Guid.NewGuid(), 7.00m, "Test Product #7", 7, 1.07);
        //Act
        var response = await _client.PutAsync(
            $"api/cart/remove-item/{cartId}",
            JsonContent.Create(bodyObject));
        //Assert
        AssertNotFound(response);
        AssertJsonProblemUtf8(response);
    }
}










