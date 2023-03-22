using System.Net.Http.Json;
using ShoppingCart.Api.Tests.Common;
using ShoppingCart.Api.Tests.ControllersTests.Extensions;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllerTests.CartControllerTests;

public class UpdateItemInCartTests : IntegrationTestBase
{
    [Theory]
    [InlineData("dfba8243-47b2-4ea8-88ac-1ab436bb0a48")] // Id from test data
    [InlineData("3eba8c48-01a6-4383-bea7-3b2876d6e2d3")] // Random id
    public async Task UpdateItemInCart_ItIsNewItem_RelevantItemIsInCartAndOkIsReturned(string id)
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.First().Id;
        Guid productId = Guid.Parse(id);
        var bodyObject = GetTestItemBody(productId, 7.00m, "Test Product #7", 7, 0.07);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/update-item/{cartId}",
            JsonContent.Create(bodyObject));
        //Assert
        response.AssertOK();
        await _cartCollection.AssertItemIsInDb(cartId, productId, 7);
    }

    [Fact]
    public async Task UpdateItemInCart_ItemIsNotValid_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        var bodyObjectWithWrongDiscount =
            GetTestItemBody(Guid.NewGuid(), 7.00m, "Test Product #7", 7, 1.07);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cartId}",
            JsonContent.Create(bodyObjectWithWrongDiscount));
        //Assert
        response.AssertBadRequest();
        response.AssertJsonProblemUtf8();
    }

    [Fact]
    public async Task UpdateItemInCart_CartDoesNotExists_ReturnsNotFound()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        var bodyObjectWithWrongDiscount =
            GetTestItemBody(Guid.NewGuid(), 7.00m, "Test Product #7", 7, 0.01);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/update-item/{cartId}",
            JsonContent.Create(bodyObjectWithWrongDiscount));
        //Assert
        response.AssertNotFound();
    }
}