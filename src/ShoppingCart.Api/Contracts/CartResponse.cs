using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Api.Contracts;

public record CartResponse(
    Guid CustomerId,
    bool IsAnonymous,
    DateTime LastModifiedDate,
    IEnumerable<CartItemResponse> Items)
{
    public static CartResponse FromEntity(Cart cart)
    {
        return new CartResponse(
            cart.Id,
            cart.IsAnonymous,
            cart.LastModifiedDate,
            cart.Items.Select(CartItemResponse.FromEntity).ToArray());
    }
}