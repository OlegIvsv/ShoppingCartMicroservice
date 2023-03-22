using System.Net;
using ShoppingCart.Api.Tests.Common;
using ShoppingCart.Api.Tests.ControllersTests.Extensions;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllerTests;


public class GuidIdConstraintTest : IntegrationTestBase
{
    [Theory]
    [InlineData("35454152-4895-42d7-b887-f274deff210d", 201)]   // Correct id
    [InlineData("35454152-4895-42d7-b887-f274deff210", 404)]    // Not uuid format
    [InlineData("00000000-0000-0000-0000-000000000000", 404)]   // Empty value
    [InlineData("", 404)]                                       // Id is omitted 
    public async Task ClearShoppingCart_AllCases_ReturnsNotFoundOrOk(string id, int status)
    {
        //Arrange
        HttpStatusCode expectedStatus = (HttpStatusCode)Enum.ToObject(typeof(HttpStatusCode), status);
        //Act
        HttpResponseMessage response = await _client.PostAsync($"api/cart/{id}", null);
        //Assert
        response.AssertStatus(expectedStatus);
    }
}