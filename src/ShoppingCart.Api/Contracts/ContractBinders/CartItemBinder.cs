using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Api.Contracts.ContractBinders;

public class CartItemBinder : IModelBinder
{
    private ModelBindingContext _bindingContext;

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        _bindingContext = bindingContext;
        try
        {
            var itemRequest = await _bindingContext.HttpContext.Request
                .ReadFromJsonAsync<CartItemRequest>();
            SetModelFromDTO(itemRequest);
        }
        catch (JsonException ex)
        {
            _bindingContext.ModelState.AddModelError(
                "ObjectFormatError",
                $"{ex.InnerException?.Message} The following json element caused a problem: {ex.Path}");
        }
    }

    private void SetModelFromDTO(CartItemRequest cartItemRequest)
    {
        var titleResult = ProductTitle.Create(cartItemRequest.ProductTitle);
        var quantityResult = Quantity.Create(cartItemRequest.ItemQuantity);
        var unitPriceResult = Money.Create(cartItemRequest.UnitPrice);
        var discountResult = Discount.Create(cartItemRequest.Discount);

        if (titleResult.IsFailed)
            _bindingContext.ModelState.AddModelError("ProductTitle", titleResult.Errors.First().Message);
        if (quantityResult.IsFailed)
            _bindingContext.ModelState.AddModelError("Quantity", quantityResult.Errors.First().Message);
        if (unitPriceResult.IsFailed)
            _bindingContext.ModelState.AddModelError("UnitPrice", unitPriceResult.Errors.First().Message);
        if (discountResult.IsFailed)
            _bindingContext.ModelState.AddModelError("Discount", discountResult.Errors.First().Message);
        if (_bindingContext.ModelState.ErrorCount != 0)
            return;

        var cartItemResult = CartItem.TryCreate(
            cartItemRequest.ProductId,
            titleResult.Value,
            quantityResult.Value,
            unitPriceResult.Value,
            discountResult.Value);

        if (cartItemResult.IsFailed)
            _bindingContext.ModelState.AddModelError("CartItem", cartItemResult.Errors.First().Message);
        else
            _bindingContext.Result = ModelBindingResult.Success(cartItemResult.Value);
    }
}