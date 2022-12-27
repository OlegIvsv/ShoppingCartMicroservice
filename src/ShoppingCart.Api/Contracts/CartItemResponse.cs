using ShoppingCart.Domain.Models;

namespace ShoppingCart.Api.Contracts
{
    public record CartItemResponse(
        int ProductId, 
        decimal UnitPrice,
        string ProductTitle,
        int Quantity);
}
