using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.ValueObjects;
using Xunit;

namespace ShoppingCart.Domain.Tests.EntitiesTests;

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
        var previousQuantity = item.ItemQuantity.Value;
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
            CreateTestItem(
                    Guid.NewGuid(), "Product_1", 1, 14.00m, 0.05),
            CreateTestItem(
                Guid.NewGuid(), "Product_2", 4, 10.50m, 0.00),
            CreateTestItem(
                Guid.NewGuid(), "Product_3", 20, 25.50m, 0.10),
            CreateTestItem(
                Guid.NewGuid(), "Product_4", 5, 100.00m, 0.00)
        };

        var cart = Cart.TryCreate(Guid.NewGuid()).Value;
        items.ForEach(i => cart.PutItem(i));
        return cart;
    }

    private CartItem CreateTestItem(
        Guid productId, 
        string title, 
        int quantity, 
        decimal price, 
        double discount,
        string imageUrl = "https://example.com/images/exaple.jpg")
    {
        return CartItem.TryCreate(
                productId,
                ProductTitle.Create(title).Value,
                Quantity.Create(quantity).Value,
                Money.Create(price).Value,
                Discount.Create(discount).Value,
                ImageUrl.Create(imageUrl).Value)
            .Value;
    }
}