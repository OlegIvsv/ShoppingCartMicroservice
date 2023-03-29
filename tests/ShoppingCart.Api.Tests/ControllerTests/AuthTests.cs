using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.Api.Tests.Common;
using ShoppingCart.Api.Tests.ControllersTests.Extensions;
using ShoppingCart.Infrastructure.Auth;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllerTests;

public class AuthTest : IntegrationTestBase
{
    [Fact]
    public async Task ClearCart_AnonymousCartAndAnonymousUser_ReturnsOk()
    {
        //Arrange
        var testCarts = await PrepareDatabase();
        Guid customerId = testCarts.First(c => c.IsAnonymous).Id;
        //Act
        HttpResponseMessage response = await _client.PutAsync($"api/cart/clear/{customerId}", null);
        //Assert
        response.AssertOK();
    }

    [Fact]
    public async Task ClearCart_RegisteredCartAndAnonymousUser_ReturnsUnauthorized()
    {
        //Arrange
        var testCarts = await PrepareDatabase();
        Guid registeredCartId = testCarts.First(c => !c.IsAnonymous).Id;
        //Act
        HttpResponseMessage response = 
            await _client.PutAsync($"api/cart/clear/{registeredCartId}", null);
        //Assert
        response.AssertUnauthorized();
    }

    [Fact]
    public async Task ClearCart_RegisteredCartAndRegisteredUser_ReturnOk()
    {
        //Arrange
        var testCarts = await PrepareDatabase();
        Guid registeredCartId = testCarts.First(c => !c.IsAnonymous).Id;
        HttpRequestMessage message = new(HttpMethod.Put, $"api/cart/clear/{registeredCartId}");
        _client.DefaultRequestHeaders.Add(
            "Authorization", 
            $"Bearer {TestTokenForId(registeredCartId.ToString())}");
        //Act
        HttpResponseMessage response = await _client.SendAsync(message);
        //Assert
        response.AssertOK();
    }

    [Fact]
    public async Task ClearCart_RegisteredCartOfOtherUser_ReturnsUnauthorized()
    {
        //Arrange
        var testCarts = await PrepareDatabase();
        Guid registeredCartId = testCarts.First(c => !c.IsAnonymous).Id;
        Guid idFromToken = Guid.NewGuid();
        HttpRequestMessage message = new(HttpMethod.Put, $"api/cart/clear/{registeredCartId}");
        _client.DefaultRequestHeaders.Add(
            "Authentication",
            $"Bearer {TestTokenForId(idFromToken.ToString())}");
        //Act
        HttpResponseMessage response = await _client.SendAsync(message);
        //Assert
        response.AssertUnauthorized();
    }

    private string TestTokenForId(string id)
    {
        var claimsDict = new Dictionary<string, object>()
        {
            {AuthSettings.IdClaimName, id}
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeTestPublicKey")),
            SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claimsDict,
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}