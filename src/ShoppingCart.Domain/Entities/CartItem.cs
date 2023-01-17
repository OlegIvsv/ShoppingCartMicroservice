using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Domain.Entities;

public sealed class CartItem : Entity<CartItem>
{
    public Guid ProductId { get; }
    public ProductTitle ProductTitle { get; }
    public Money UnitPrice { get; }
    public Quantity ItemQuantity { get; private set; }
    public Discount Discount { get; }

    private CartItem(
        Guid productId,
        ProductTitle productTitle,
        Quantity quantity,
        Money unitPrice,
        Discount discount)
    {
        if (productTitle is null || quantity is null ||
            unitPrice is null || discount is null)
            throw new ArgumentNullException();
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductTitle = productTitle;
        UnitPrice = unitPrice;
        ItemQuantity = quantity;
        Discount = discount;
    }

    public static Result<CartItem> TryCreate(
        Guid productId,
        ProductTitle productTitle,
        Quantity quantity,
        Money unitPrice,
        Discount discount)
    {
        if (productId == Guid.Empty)
            return Result.Fail(new InvalidIdValueError(productId));
        return new CartItem(productId, productTitle, quantity, unitPrice, discount);
    }

    public static CartItem Create(
        Guid productId,
        ProductTitle productTitle,
        Quantity quantity,
        Money unitPrice,
        Discount discount)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Invalid id value", nameof(productId));
        return new CartItem(productId, productTitle, quantity, unitPrice, discount);
    }

    public void CorrectQuantityWith(Quantity quantityChange)
    {
        if (quantityChange is null)
            throw new ArgumentNullException(nameof(quantityChange));
        var newQuantity = Quantity.Add(ItemQuantity, quantityChange);
        ItemQuantity = newQuantity;
    }

    public void SetQuantity(Quantity newQuantity)
    {
        if (newQuantity is null)
            throw new ArgumentNullException(nameof(newQuantity));
        ItemQuantity = newQuantity;
    }
}