using Moq;
using ShoppingCart.Application.Errors;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Models;
using ShoppingCart.Infrastructure.DataAccess;
using System.IO;
using Xunit;

namespace ShoppingCart.Application.Tests.ServicesTests;
public class ShoppingCartServiceTests
{
    private Mock<IShoppingCartRepository> _repositoryMock;

    public ShoppingCartServiceTests()
    {
        _repositoryMock = new Mock<IShoppingCartRepository>();
    }

    [Fact]
    public async Task GetCartByCustomer_CartExists_ReturnsCart()
    {
        //Arrange
        var cart = new Cart(1);
        _repositoryMock
            .Setup(repo => repo.FindByCustomer(It.IsAny<int>()))
            .ReturnsAsync(cart);
        var service = new ShoppingCartService(_repositoryMock.Object);

        //Act
        var result = await service.GetCartByCustomer(1);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Id, cart.Id);
    }


    [Fact]
    public async Task GetCartByCustomer_CartDoesNotExists_ReturnsErrorResult()
    {
        //Arrange
        _repositoryMock
            .Setup(repo => repo.FindByCustomer(It.IsAny<int>()))
            .ReturnsAsync((Cart)null);
        var service = new ShoppingCartService(_repositoryMock.Object);

        //Act
        var result = await service.GetCartByCustomer(1);

        //Assert
        Assert.True(result.IsFailed);
        Assert.IsType<CartDoesNotExistsError>(result.Errors[0]);
    }

    [Fact]
    public async Task CreateCart_CartDoesNotExists_ReturnsCreatedCart()
    {
        //Arrange
        int customerId = 1;
        _repositoryMock
            .Setup(repo => repo.Add(It.IsAny<Cart>()))
            .ReturnsAsync(new Cart(customerId));
        var service = new ShoppingCartService(_repositoryMock.Object);

        //Act
        var result = await service.CreateCart(customerId);

        //Assert
        Assert.NotNull(result.Value);
        _repositoryMock.Verify(
            repo => repo.Add(It.Is<Cart>(cart => cart.CustomerId == customerId)));
    }

    [Fact]
    public async Task CreateCart_CartAlreadyExists_ReturnsError()
    {
        //Arrange
        int customerId = 1;
        _repositoryMock
            .Setup(repo => repo.FindByCustomer(customerId))
            .ReturnsAsync(new Cart(customerId));
        var service = new ShoppingCartService(_repositoryMock.Object);

        //Act
        var result = await service.CreateCart(customerId);

        //Assert
        Assert.True(result.IsFailed);
        _repositoryMock.Verify(
            repo => repo.Add(It.IsAny<Cart>()), Times.Never());
    }
}
