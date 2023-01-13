using ShoppingCart.Domain.ValueObjects;
using Xunit;

namespace ShoppingCart.Domain.Tests.ValueObjectsTests;
public class QuantityTests
{
    [Theory]
    [InlineData(10, 5, 15)]                                 // The sum is in the range of acceptible values
    [InlineData(90, 10, Quantity.MaxQuantityForProduct)]    // The sum is equal to the max acceptible value
    [InlineData(90, 15, Quantity.MaxQuantityForProduct)]    // The sum is greater than the max value
    public void Add_VariousCases_ReturnsValuesInAcceptibleRange(int first, int second, int result)
    {
        //Arrange
        Quantity firstQuantity = Quantity.Create(first).Value;
        Quantity secondQuantity = Quantity.Create(second).Value;
        //Act
        Quantity resultQuantity = Quantity.Add(firstQuantity, secondQuantity);
        //Assert
        Assert.Equal(result, resultQuantity.Value);
    }
}
