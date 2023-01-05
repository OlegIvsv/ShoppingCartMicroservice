using ShoppingCart.Domain.Models;

namespace ShoppingCart.Api.Contracts
{
    public record CartResponse(
        Guid CustomerId,
        IEnumerable<CartItemResponse> Items);
}
