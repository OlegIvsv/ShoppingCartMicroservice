using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Api.Controllers;
using ShoppingCart.Application.Errors;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Models;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests;

public class CartsControllerTests
{
    private Mock<IShoppingCartService> _serviceMock;

    public CartsControllerTests()
    {
        _serviceMock = new Mock<IShoppingCartService>();
    }

    [Fact]
    public async Task GetShoppingCartByCustomerId_ExistingCartId_ReturnsOk()
    {
        //Arrange
        int customerId = 1;
        Cart cart = new Cart(customerId);
        _serviceMock.Setup(cs => cs.GetCartByCustomer(customerId))
            .ReturnsAsync(cart);
        var controller = new CartController(_serviceMock.Object);

        //Act
        var result = await controller.GetShoppingCartByCustomerId(customerId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseModel = Assert.IsType<CartResponse>(okResult.Value); 
        Assert.Equal(responseModel.CustomerId, customerId);
    }

    [Fact]
    public async Task GetShoppingCartByCustomerId_UnexistingCartId_Returns404()
    {
        //Arrange
        int customerId = 1;
        _serviceMock.Setup(cs => cs.GetCartByCustomer(customerId))
            .ReturnsAsync(Result.Fail(new CartDoesNotExistsError(customerId)));
        var controller = new CartController(_serviceMock.Object);

        //Act
        var result = await controller.GetShoppingCartByCustomerId(customerId);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(problemDetails.Status, StatusCodes.Status404NotFound);
    }
}

