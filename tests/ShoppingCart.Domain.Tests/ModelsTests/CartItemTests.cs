using ShoppingCart.Domain.Errors;
using ShoppingCart.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace ShoppingCart.Domain.Tests.ModelsTests;
public class CartItemTests
{
    [Fact]
    public void SetQuantity_ValidValue_QuantityChanges()
    {
        //Arrange
        var item = new CartItem(1, "Test Product", 10.00m, 10);
        int newQuantity = 5;
        
        //Act
        item.SetQuantity(newQuantity);

        //Assert
        Assert.Equal(newQuantity, item.Quantity);
    }

    [Fact]
    public void SetQuantity_InvalidValue_QuantityChanges()
    {
        //Arrange
        var item = new CartItem(1, "Test Product", 10.00m, 10);

        //Act
        var action = () => item.SetQuantity(-1);

        //Assert
        Assert.ThrowsAny<CartItemErrors.QuantityException>(action);
    }
}

