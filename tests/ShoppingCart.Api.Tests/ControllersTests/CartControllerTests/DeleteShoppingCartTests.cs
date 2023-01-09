using ShoppingCart.Api.Tests.ControllersTests.CartControllerTests;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests;

public class DeleteShoppingCartTests : CartsControllerIntegrationTestsBase
{

    [Fact]
    public async Task DeleteShoppingCart_CartExists_ReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last().Id;
        //Act
        var response = await _client.DeleteAsync($"api/cart/{cartId}");
        //Assert
        AssertOK(response);
    }

    [Fact]
    public async Task DeleteShoppingCart_CartDoesNotExist_NotFound()
    {
        //Arrange
        await PrepareDatabase();
        Guid randomId = Guid.NewGuid();
        //Act
        var response = await _client.DeleteAsync($"api/cart/{randomId}");
        //Assert
        AssertNotFound(response);
        AssertJsonProblemUtf8(response);
    }

    [Fact]
    public async Task DeleteShoppingCart_InvalidId_ReturnsBadRequest()
    {
        //Arrange
        await PrepareDatabase();
        //Act
        var response = await _client.DeleteAsync($"api/cart/{Guid.Empty}");
        //Assert
        AssertBadRequest(response);
        AssertJsonProblemUtf8(response);
    }

}










