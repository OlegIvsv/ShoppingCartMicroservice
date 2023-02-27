using Swashbuckle.AspNetCore.Filters;

namespace ShoppingCart.Api.Contracts.Swagger;

public class CartItemExampleProvider : IExamplesProvider<CartItemRequest>
{
    public CartItemRequest GetExamples()
        => new CartItemRequest(Guid.Empty, 0.00m, "string", 0, 0.00, "string");
}