using System.Net.Http.Json;
using ShoppingCart.Api.Tests.Common;
using ShoppingCart.Api.Tests.ControllersTests.Extensions;
using ShoppingCart.Domain.Entities;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllerTests.CartControllerTests;

public class PutItemToCartTests : IntegrationTestBase
{
    [Fact]
    public async Task PutItemToCart_ItIsNewItem_AddsItemAndReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.First().Id;
        var productId = Guid.NewGuid();
        var bodyObject = GetTestItemBody(productId, 7.00m, "Test Product #7", 7, 0.07);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cartId}",
            JsonContent.Create(bodyObject));
        //Assert
        response.AssertOK();
        await _cartCollection.AssertItemIsInDb(cartId, productId, 7);
    }

    [Fact]
    public async Task PutItemToCart_ItemAlreadyInCart_ChangesQuantityAndReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Cart? cart = cartsInDb.First();
        CartItem? item = cartsInDb.First().Items.Last();
        var bodyObject = GetTestItemBody(item.ProductId, 7.00m, "Test Product #7", 7, 0.07);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cart.Id}",
            JsonContent.Create(bodyObject));
        //Assert
        response.AssertOK();
        await _cartCollection.AssertItemIsInDb(cart.Id, item.ProductId, item.ItemQuantity.Value + 7);
    }

    [Fact]
    public async Task PutItemToCart_ItemIsNotValid_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        var bodyObjectWithWrongDiscount =
            GetTestItemBody(Guid.NewGuid(), 7.00m, "Test Product #7", 7, 1.07); // discount = 107%
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cartId}",
            JsonContent.Create(bodyObjectWithWrongDiscount));
        //Assert
        response.AssertBadRequest();
        response.AssertJsonProblemUtf8();
    }

    [Fact]
    public async Task PutItemToCart_CartDoesNotExists_ReturnsNotFound()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        var bodyObjectWithWrongDiscount =
            GetTestItemBody(Guid.NewGuid(), 7.00m, "Test Product #7", 7, 0.01);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cartId}",
            JsonContent.Create(bodyObjectWithWrongDiscount));
        //Assert
        response.AssertNotFound();
    }
}