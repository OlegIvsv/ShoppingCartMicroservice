using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;
using Xunit;

namespace ShoppingCart.Domain.Tests.ModelsTests;
public class CartTests
{
    [Fact]
    public void PutItem_PutNewItem_AddToItemsAndReturnsTrue()
    {
        //Arrange
        var cart = CreateTestCart();
        var newCartItem = CreateTestItem(Guid.NewGuid(), "TestProduct_5", 5, 4.00m, 0.01);
        //Act
        cart.PutItem(newCartItem);
        //Assert
        Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
    }

    [Fact]
    public void PutItem_PutExistingItem_ReplacesItemAndReturnsFalse()
    {
        //Arrange
        var cart = CreateTestCart();
        var item = cart.Items.First();
        int previousQuantity = item.Quantity.Value;
        var newCartItem = CreateTestItem(item.ProductId, "TestProduct_4", 4, 4.00m, 0.01);
        //Act
        cart.PutItem(newCartItem);
        //Assert
        var itemAfter = Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.Equal(itemAfter.Quantity.Value, previousQuantity + 4);
    }

    [Fact]
    public void RemoveItem_RemoveExistingItem_RemovesItem()
    {
        //Arrange
        var cart = CreateTestCart();
        Guid itemToRemoveId = cart.Items.Skip(2).First().ProductId;  
        //Act
        cart.RemoveItem(itemToRemoveId);
        //Assert
        Assert.DoesNotContain(cart.Items, item => item.ProductId == itemToRemoveId);
    }

    [Fact]
    public void RemoveItem_RemoveUnexistingItem_ThrowsException()
    {
        //Arrange
        var cart = CreateTestCart();
        Guid itemToRemoveId = cart.Items.Skip(2).First().ProductId;
        int sizeBefore = cart.Items.Count;
        //Act
        var action = () => cart.RemoveItem(itemToRemoveId);
        //Assert
        Assert.Equal(sizeBefore, cart.Items.Count);
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
