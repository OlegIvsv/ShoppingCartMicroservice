using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Api.Contracts;

public record CartResponse(Guid CustomerId, IEnumerable<CartItemResponse> Items)
{
    public static CartResponse FromEntity(Cart cart)
    {
        return new CartResponse(
            cart.Id,
            cart.Items.Select(CartItemResponse.FromEntity).ToArray());
    }
}