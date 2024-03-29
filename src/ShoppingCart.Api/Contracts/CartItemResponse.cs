﻿using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Api.Contracts;

public record CartItemResponse(
    Guid Id,
    Guid ProductId,
    decimal UnitPrice,
    string ProductTitle,
    int ItemQuantity,
    double Discount,
    string ImageUrl)
{
    public static CartItemResponse FromEntity(CartItem item)
    {
        return new CartItemResponse(
            item.Id,
            item.ProductId,
            item.UnitPrice.Value,
            item.ProductTitle.Value,
            item.ItemQuantity.Value,
            item.Discount.Value,
            item.Image.Value);
    }
}