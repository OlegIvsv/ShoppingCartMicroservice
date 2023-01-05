namespace ShoppingCart.Api.Contracts
{
    public record CartItemRequest(
        Guid ProductId,
        decimal UnitPrice,
        string ProductTitle,
        int Quantity,
        double Discount);
}
