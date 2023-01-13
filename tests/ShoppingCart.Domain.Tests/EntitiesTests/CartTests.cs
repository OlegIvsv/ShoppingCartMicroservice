using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;
using Xunit;

namespace ShoppingCart.Domain.Tests.ModelsTests;

public class CartTests
{
    [Fact]
    public void PutItem_NewItem_AddToItems()
    {
        //Arrange
        var cart = CreateTestCart();
        var newCartItem = CreateTestItem(Guid.NewGuid(), "Test Product", 5, 4.00m, 0.01);
        //Act
        cart.PutItem(newCartItem);
        //Assert
        var itemAfter = Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.Equal(newCartItem.ItemQuantity, itemAfter.ItemQuantity);
    }

    [Fact]
    public void PutItem_ExistingItem_CorrectsItemQuantity()
    {
        //Arrange
        var cart = CreateTestCart();
        var item = cart.Items.First();
        int previousQuantity = item.ItemQuantity.Value;
        var newCartItem = CreateTestItem(item.ProductId, "Test Product", 4, 4.00m, 0.01);
        //Act
        cart.PutItem(newCartItem);
        //Assert
        var itemAfter = Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.Equal(previousQuantity + 4, itemAfter.ItemQuantity.Value);
    }

    [Fact]
    public void UpdateItem_ExistingItem_UpdatesItemQuantity()
    {
        //Arrange
        var cart = CreateTestCart();
        var item = cart.Items.First();
        var newCartItem = CreateTestItem(item.ProductId, "Test Product", 4, 4.00m, 0.01);
        //Act
        cart.UpdateItem(newCartItem);
        //Assert
        var itemAfter = Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.Equal(newCartItem.ItemQuantity, itemAfter.ItemQuantity);
    }

    [Fact]
    public void UpdateItem_ItemDoesNotExists_AddsItemToCart()
    {
        //Arrange
        var cart = CreateTestCart();
        var newCartItem = CreateTestItem(Guid.NewGuid(), "Test Product", 4, 4.00m, 0.01);
        //Act
        cart.UpdateItem(newCartItem);
        //Assert
        var itemAfter = Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.Equal(4, itemAfter.ItemQuantity.Value);
    }

    private Cart CreateTestCart()
    {
        var items = new List<CartItem>()
        {
            CartItem.Create(
                Guid.NewGuid(),
                ProductTitle.Create("Product_1").Value,
                Quantity.Create(1).Value,
                Money.Create(14.00m).Value,
                Discount.Create(0.05).Value)
            .Value,

            CartItem.Create(
                Guid.NewGuid(),
                ProductTitle.Create("Product_2").Value,
                Quantity.Create(4).Value,
                Money.Create(10.50m).Value,
                Discount.Create(0.00).Value)
            .Value,

            CartItem.Create(
                Guid.NewGuid(),
                ProductTitle.Create("Product_3").Value,
                Quantity.Create(20).Value,
                Money.Create(25.50m).Value,
                Discount.Create(0.10).Value)
            .Value,

            CartItem.Create(
                Guid.NewGuid(),
                ProductTitle.Create("Product_4").Value,
                Quantity.Create(5).Value,
                Money.Create(100.00m).Value,
                Discount.Create(0.00).Value)
            .Value,
        };

        var cart = Cart.Create(Guid.NewGuid()).Value;
        items.ForEach(i => cart.PutItem(i));
        return cart;
    }

    private CartItem CreateTestItem(Guid productId, string title, int quantity, decimal price, double discount)
    {
        return CartItem.Create(
                productId,
                ProductTitle.Create(title).Value,
                Quantity.Create(quantity).Value,
                Money.Create(price).Value,
                Discount.Create(discount).Value)
            .Value;
    }
}
