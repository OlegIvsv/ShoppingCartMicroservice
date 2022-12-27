using ShoppingCart.Domain.Models;

namespace ShoppingCart.Api.Contracts
{
    public record CartResponse(
        string Id,
        int CustomerId,
        IList<CartItemResponse> Items);   
}
