using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;
using ShoppingCart.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace ShoppingCart.Domain.Models
{
    public sealed class CartItem : Entity<CartItem>
    {
        public Guid ProductId { get; private init; }
        public ProductTitle ProductTitle { get; private init; }
        public Money UnitPrice { get; private init; }
        public Quantity Quantity { get; private set; }
        public Discount Discount { get; private init; }
         
        private CartItem(
            Guid productId, 
            ProductTitle productTitle, 
            Quantity quantity, 
            Money unitPrice, 
            Discount discount)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductTitle = productTitle;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
        }

        public static Result<CartItem> Create(
            Guid productId, 
            ProductTitle productTitle, 
            Quantity quantity, 
            Money unitPrice, 
            Discount discount)
        {
            if(productTitle is null
                || quantity is null
                || unitPrice is null 
                || discount is null)
                throw new ArgumentNullException("Arguments can't be null here");

            if (productId == Guid.Empty)
                return Result.Fail(new InvalidIdValueError(productId));

            return new CartItem(productId, productTitle, quantity, unitPrice, discount);
        }

        public void SetQuantity(Quantity newQuantity)
        {
            if(newQuantity is null)
                throw new ArgumentNullException("Quantity can't be null here");

            Quantity = newQuantity;
        }
    }
}
