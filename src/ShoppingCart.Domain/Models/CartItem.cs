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
        public int ProductId { get; init; }
        public string ProductTitle { get; init; }
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }

        private CartItem(int productId, string productTitle, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            ProductTitle = productTitle;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public static CartItem Create(int productId, string productTitle, decimal unitPrice, int quantity)
        {
            if (unitPrice <= 0)
                throw new ValidationException("Invalid unit price!");
            if (string.IsNullOrWhiteSpace(productTitle))
                throw new ValidationException("Invalid product title!");
            if (quantity < 1)
                throw new ValidationException("Invalid product title!");

            return new(productId, productTitle, unitPrice, quantity);
        }
    }
}
