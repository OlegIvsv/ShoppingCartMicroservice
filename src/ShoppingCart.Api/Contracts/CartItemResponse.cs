using ShoppingCart.Domain.Models;

namespace ShoppingCart.Api.Contracts
{
    public record CartItemResponse(
         Guid Id,
         Guid ProductId,
         decimal UnitPrice,
         string ProductTitle,
         int Quantity,
         double Discount);
}
