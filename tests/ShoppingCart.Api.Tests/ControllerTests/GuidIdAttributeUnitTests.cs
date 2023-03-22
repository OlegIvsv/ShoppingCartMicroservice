using ShoppingCart.Api.Contracts.ContractAttributes;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllerTests;

public class GuidIdAttributeTests
{
    [Theory]
    [InlineData("invalid-guid")]
    [InlineData(null)]
    public void IsValid_ReturnsFalse_ForInvalidGuids(object value)
    {
        // Arrange
        var attribute = new GuidIdAttribute();
        // Act
        var result = attribute.IsValid(value);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_ForEmptyGuid()
    {
        // Arrange
        var attribute = new GuidIdAttribute();
        var value = Guid.Empty;
        // Act
        var result = attribute.IsValid(value);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsTrue_ForValidGuid()
    {
        // Arrange
        var attribute = new GuidIdAttribute();
        var value = Guid.NewGuid();
        // Act
        var result = attribute.IsValid(value);
        // Assert
        Assert.True(result);
    }
}
