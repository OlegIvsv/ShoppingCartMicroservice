using MongoDB.Driver;
using ShoppingCart.Domain.Entities;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.Extensions;

internal static class DatabaseAssertExtensions
{
    public static async Task AssertItemIsInDb(
        this IMongoCollection<Cart> collection,
        Guid cartId,
        Guid productId,
        int expectedQuantity)
    {
        Cart? cart = await collection.Find(c => c.Id == cartId).FirstAsync();
        var result = cart.Items.Any(
            item => item.ProductId == productId
                    && item.ItemQuantity.Value == expectedQuantity);
        Assert.True(result, "Test database doesn't contain a product with these Id and Quantity");
    }

    public static async Task AssertItemIsNotInDb(
        this IMongoCollection<Cart> collection,
        Guid cartId,
        Guid productId)
    {
        Cart? cart = await collection.Find(c => c.Id == cartId).FirstAsync();
        var result = cart.Items.Any(item => item.ProductId == productId);
        Assert.False(result, "Test database contains a product with these Id");
    }
}