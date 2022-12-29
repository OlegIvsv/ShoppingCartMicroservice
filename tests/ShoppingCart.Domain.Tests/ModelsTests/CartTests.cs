using ShoppingCart.Domain.Errors;
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
        var newCartItem = new CartItem(5, "TestProduct_5", 4.00m, 10);

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
        var newCartItem = new CartItem(4, "TestProduct_4", 4.00m, 10);

        //Act
        bool putResult = cart.PutItem(newCartItem);

        //Assert
        Assert.Single(cart.Items, p => p.ProductId == newCartItem.ProductId);
        Assert.False(putResult);
    }

    [Fact]
    public void RemoveItem_RemoveExistingItem_RemovesItem()
    {
        //Arrange
        var cart = CreateTestCart();
        var itemToRemove = new CartItem(2, "TestProduct_2", 4.00m, 10);

        //Act
        cart.RemoveItem(itemToRemove.ProductId);

        //Assert
        Assert.DoesNotContain(cart.Items, item => item.ProductId == itemToRemove.ProductId);
    }

    [Fact]
    public void RemoveItem_RemoveUnexistingItem_ThrowsException()
    {
        //Arrange
        var cart = CreateTestCart();
        var itemToRemove = new CartItem(5, "TestProduct_2", 4.00m, 10);

        //Act
        var action = () => cart.RemoveItem(itemToRemove.ProductId);

        //Assert
        Assert.Throws<CartErrors.CartDoesNotContainItemException>(action);
    }

    private Cart CreateTestCart()
    {
        var items = new List<CartItem>()
        {
            new CartItem(1, "TestProduct_1", 10.00m, 5),
            new CartItem(2, "TestProduct_2", 10.00m, 1),
            new CartItem(3, "TestProduct_3", 10.00m, 3),
            new CartItem(4, "TestProduct_4", 10.00m, 10),
        };

        return new Cart(Guid.NewGuid().ToString(), 1, items);
    }
}
