using ShoppingCart.Domain.Errors;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Validators
{
    internal class CartItemValidator
    {
        public const int MaxQuantityForProduct = 100;
        public const int MaxTitleLength = 75;
        public const decimal MinUnitPrice = 0.01m;

        public static void IsValidUnitPrice(decimal price)
        {
            if (price < MinUnitPrice)
                throw new CartItemErrors.PriceException();
        }

        public static void IsValidTitle(string titleValue)
        {
            if(string.IsNullOrWhiteSpace(titleValue) || titleValue.Length > MaxTitleLength)
                throw new CartItemErrors.ProductTitleException();
        }

        public static void IsValidQuantity(int quantityValue)
        {
            if(quantityValue is > MaxQuantityForProduct or <= 0)
                throw new CartItemErrors.QuantityException();
        }

        public static void IsValidItem(CartItem item)
        {
            IsValidUnitPrice(item.UnitPrice);
            IsValidQuantity(item.Quantity);
            IsValidTitle(item.ProductTitle);
        }
    }
}
