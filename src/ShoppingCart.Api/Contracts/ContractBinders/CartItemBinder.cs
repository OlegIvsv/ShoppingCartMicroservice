using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Api.Contracts.ContractBinders;

public class CartItemBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        try
        {
            var itemRequest = await bindingContext.HttpContext.Request
                .ReadFromJsonAsync<CartItemRequest>();
            SetModelFromDTO(itemRequest, bindingContext);
        }
        catch (JsonException ex)
        {
            bindingContext.ModelState.AddModelError(
                "ObjectFormatError",
                $"{ex.InnerException?.Message} The following json element caused a problem: {ex.Path}");
        }
    }

    private void SetModelFromDTO(CartItemRequest cartItemRequest, ModelBindingContext ctx)
    {
        var titleResult = ProductTitle.Create(cartItemRequest.ProductTitle);
        var quantityResult = Quantity.Create(cartItemRequest.ItemQuantity);
        var unitPriceResult = Money.Create(cartItemRequest.UnitPrice);
        var discountResult = Discount.Create(cartItemRequest.Discount);

        if (titleResult.IsFailed)
            ctx.ModelState.AddModelError("ProductTitle", titleResult.Errors.First().Message);
        if (quantityResult.IsFailed)
            ctx.ModelState.AddModelError("Quantity", quantityResult.Errors.First().Message);
        if (unitPriceResult.IsFailed)
            ctx.ModelState.AddModelError("UnitPrice", unitPriceResult.Errors.First().Message);
        if (discountResult.IsFailed)
            ctx.ModelState.AddModelError("Discount", discountResult.Errors.First().Message);
        if (ctx.ModelState.ErrorCount != 0)
            return;

        var cartItemResult = CartItem.TryCreate(
            cartItemRequest.ProductId,
            titleResult.Value,
            quantityResult.Value,
            unitPriceResult.Value,
            discountResult.Value);

        if (cartItemResult.IsFailed)
            ctx.ModelState.AddModelError("CartItem", cartItemResult.Errors.First().Message);
        else
            ctx.Result = ModelBindingResult.Success(cartItemResult.Value);
    }

    private record CartItemRequest(
        Guid ProductId,
        decimal UnitPrice,
        string ProductTitle,
        int ItemQuantity,
        double Discount);
}