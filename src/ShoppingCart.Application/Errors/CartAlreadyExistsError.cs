using FluentResults;
using System;

namespace ShoppingCart.Application.Errors
{
    public class CartAlreadyExistsError : Error
    {
        public CartAlreadyExistsError(int customerId)
           : base($"Customer with id {customerId} already has a shopping cart")
        {
        }

    }
}
