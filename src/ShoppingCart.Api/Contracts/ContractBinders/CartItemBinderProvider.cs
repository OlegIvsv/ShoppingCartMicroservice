using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Api.Contracts.ContractBinders;

public class CartItemBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));
        return context.Metadata.ModelType != typeof(CartItem) ? null : new CartItemBinder();
    }
}