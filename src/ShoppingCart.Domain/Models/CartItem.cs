using FluentResults;
using ShoppingCart.Domain.Errors;
using ShoppingCart.Domain.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Models
{
    public sealed class CartItem
    {
        public Guid Id { get; private init; }
        public int ProductId { get; private init; }
        public string ProductTitle { get; private init; }
        public decimal UnitPrice { get; private init; }
        public int Quantity { get; private set; }
         
        public CartItem(int productId, string productTitle, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            ProductTitle = productTitle;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Id = Guid.NewGuid();

            CartItemValidator.IsValidItem(this);
        }

        public void SetQuantity(int quantity)
        {
            CartItemValidator.IsValidQuantity(quantity);
            Quantity = quantity;
        }
    }
}
