using ShoppingCart.Domain.Models;
using System.IO;
using Xunit;

namespace ShoppingCart.Domain.Tests.ModelsTests;
public class CartTests
{
    [Fact]
    public void PutItem_PutNewItem_AddToItemsAndReturnsTrue()
    {
        //Arrange
        var cart = CreateTestCart();
        var newCartItem = CartItem.Create(5, "TestProduct_5", 4.00m, 10);

        //Act
        bool putResult = cart.PutItem(newCartItem);

        //Assert
        Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.True(putResult);
    }

    [Fact]
    public void PutItem_PutExistingItem_ReplacesItemAndReturnsFalse()
    {
        //Arrange
        var cart = CreateTestCart();
        var newCartItem = CartItem.Create(4, "TestProduct_4", 4.00m, 10);

        //Act
        bool putResult = cart.PutItem(newCartItem);

        //Assert
        Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.False(putResult);
    }

    [Fact]
    public void RemoveItem_RemoveExistingItem_ReturnsTrue()
    {
        //Arrange
        var cart = CreateTestCart();
        var itemToRemove = CartItem.Create(2, "TestProduct_2", 4.00m, 10);

        //Act
        var removeResult = cart.RemoveItem(itemToRemove.ProductId);

        //Assert
        Assert.DoesNotContain(itemToRemove, cart.Items);
        Assert.True(removeResult);
    }

    [Fact]
    public void RemoveItem_RemoveUnexistingItem_ReturnsFalse()
    {
        //Arrange
        var cart = CreateTestCart();
        var itemToRemove = CartItem.Create(5, "TestProduct_2", 4.00m, 10);

        //Act
        var removeResult = cart.RemoveItem(itemToRemove.ProductId);

        //Assert
        Assert.False(removeResult);
    }

    private Cart CreateTestCart()
    {
        var items = new List<CartItem>()
        {
            CartItem.Create(1, "TestProduct_1", 10.00m, 5),
            CartItem.Create(2, "TestProduct_2", 10.00m, 1),
            CartItem.Create(3, "TestProduct_3", 10.00m, 3),
            CartItem.Create(4, "TestProduct_4", 10.00m, 10),
        };

        return new Cart(Guid.NewGuid().ToString(), 1, items);
    }
}
