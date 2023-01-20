using ShoppingCart.Domain.ValueObjects;
using Xunit;

namespace ShoppingCart.Domain.Tests.ValueObjectsTests;

public class QuantityTests
{
    [Theory]
    [InlineData(10, 5, 15)] // The sum is in the range of acceptable values
    [InlineData(90, 10, 100)] // The sum is equal to the max acceptable value
    [InlineData(90, 15, 100)] // The sum is greater than the max value
    public void Add_VariousCases_ReturnsValuesInAcceptibleRange(int first, int second, int result)
    {
        //Arrange
        var firstQuantity = Quantity.Create(first).Value;
        var secondQuantity = Quantity.Create(second).Value;
        //Act
        var resultQuantity = Quantity.Add(firstQuantity, secondQuantity);
        //Assert
        Assert.Equal(result, resultQuantity.Value);
    }
}