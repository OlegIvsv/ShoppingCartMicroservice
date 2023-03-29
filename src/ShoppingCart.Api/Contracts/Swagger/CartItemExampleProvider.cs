using Swashbuckle.AspNetCore.Filters;

namespace ShoppingCart.Api.Contracts.Swagger;


public class CartItemExampleProvider : IExamplesProvider<object>
{
    public object GetExamples() => new
    {
        customerId = Guid.Empty,
        unitPrice = 0.00m,
        productTitle = "string",
        itemQuantity = 0,
        discount = 0.00,
        imageUrl = "string"
    };
}