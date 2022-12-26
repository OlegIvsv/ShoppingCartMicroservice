using ShoppingCart.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Xunit;

namespace ShoppingCart.Domain.Tests.ModelsTests;
public class CartItemTests
{
    [Theory]
    [InlineData(0.00)]
    [InlineData(-1.00)]
    public void Create_InvalidPrice_ThrowsException(decimal price)
    {
        //Act
        var action = () => CartItem.Create(1, "Test Product", price, 1);
        //Assert
        Assert.Throws<ValidationException>(action);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidTitle_ThrowsException(string productTitle)
    {
        //Act
        var action = () => CartItem.Create(1, productTitle, 1.00m, 1);
        //Assert
        Assert.Throws<ValidationException>(action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidQuantity_ThrowsException(int quantity)
    {
        //Act
        var action = () => CartItem.Create(1, "Test Product", 1.00m, quantity);
        //Assert
        Assert.Throws<ValidationException>(action);
    }
}

