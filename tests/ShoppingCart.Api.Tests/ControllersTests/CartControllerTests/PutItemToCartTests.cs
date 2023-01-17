using System.Net.Http.Json;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Domain.Entities;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;

public class PutItemToCartTests : CartsControllerIntegrationTestsBase
{
    [Fact]
    public async Task PutItemToCart_ItIsNewItem_AddsItemAndReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.First().Id;
        var productId = Guid.NewGuid();
        CartItemRequest bodyObject = new(productId, 7.00m, "Test Product #7", 7, 0.07);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cartId}",
            JsonContent.Create(bodyObject));
        //Assert
        AssertOK(response);
        await AssertItemIsInDb(cartId, productId, 7);
    }

    [Fact]
    public async Task PutItemToCart_ItemAlreadyInCart_ChangesQuantityAndReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Cart? cart = cartsInDb.First();
        CartItem? item = cartsInDb.First().Items.Last();
        CartItemRequest bodyObject = new(item.ProductId, 7.00m, "Test Product #7", 7, 0.07);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cart.Id}",
            JsonContent.Create(bodyObject));
        //Assert
        AssertOK(response);
        await AssertItemIsInDb(cart.Id, item.ProductId, item.ItemQuantity.Value + 7);
    }

    [Fact]
    public async Task PutItemToCart_ItemIsNotValid_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        var cartId = Guid.NewGuid();
        CartItemRequest bodyObjectWithWrongDiscount =
            new(Guid.NewGuid(), 7.00m, "Test Product #7", 7, 1.07);
        //Act
        HttpResponseMessage response = await _client.PutAsync(
            $"api/cart/put-item/{cartId}",
            JsonContent.Create(bodyObjectWithWrongDiscount));
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }
}