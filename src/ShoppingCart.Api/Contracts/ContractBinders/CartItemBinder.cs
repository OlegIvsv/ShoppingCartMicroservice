using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Api.Contracts.ContractBinders;   

public class CartItemBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var cartItemRequest = await bindingContext.HttpContext.Request
            .ReadFromJsonAsync<CartItemRequest>();
        
        var titleResult = ProductTitle.Create(cartItemRequest.ProductTitle);
        var quantityResult = Quantity.Create(cartItemRequest.ItemQuantity);
        var unitPriceResult = Money.Create(cartItemRequest.UnitPrice);
        var discountResult = Discount.Create(cartItemRequest.Discount);
    
        if (titleResult.IsFailed)
            PutError("ProductTitle", titleResult);
        if (quantityResult.IsFailed)
            PutError("Quantity", quantityResult);
        if (unitPriceResult.IsFailed)
            PutError("UnitPrice", unitPriceResult);
        if (discountResult.IsFailed)
            PutError("Discount", discountResult);
        if (bindingContext.ModelState.ErrorCount != 0)
            return;

        var cartItemResult = CartItem.TryCreate(
            cartItemRequest.ProductId,
            titleResult.Value,
            quantityResult.Value,
            unitPriceResult.Value,
            discountResult.Value);

        if (cartItemResult.IsFailed)
            PutError("CartItem", cartItemResult); 
        else 
            bindingContext.Result = ModelBindingResult.Success(cartItemResult.Value);
        
        void PutError<T>(string title, Result<T> result)        
        {
            bindingContext.ModelState.AddModelError(title, result.Errors.First().Message);
        }
    }
}