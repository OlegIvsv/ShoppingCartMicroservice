using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Api.Contracts.ContractBinders;

public class CartItemBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context is null) 
            throw new ArgumentNullException(nameof(context));
        if (context.Metadata.ModelType != typeof(CartItem))
            return null;
        return new CartItemBinder();
    }
}