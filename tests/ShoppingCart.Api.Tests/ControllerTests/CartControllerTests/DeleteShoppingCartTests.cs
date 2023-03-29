using ShoppingCart.Api.Tests.Common;
using ShoppingCart.Api.Tests.ControllersTests.Extensions;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllerTests.CartControllerTests;

public class DeleteShoppingCartTests : IntegrationTestBase
{
    [Fact]
    public async Task DeleteShoppingCart_CartExists_ReturnsOk()
    {
        //Arrange
        var cartsInDb = await PrepareDatabase();
        Guid cartId = cartsInDb.Last(c => c.IsAnonymous).Id;
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"api/cart/{cartId}");
        //Assert
        response.AssertOK();
    }

    [Fact]
    public async Task DeleteShoppingCart_CartDoesNotExist_NotFound()
    {
        //Arrange
        await PrepareDatabase();
        var randomId = Guid.NewGuid();
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"api/cart/{randomId}");
        //Assert
        response.AssertNotFound();
        response.AssertJsonProblemUtf8();
    }
}