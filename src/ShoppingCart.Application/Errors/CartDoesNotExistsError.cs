using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Errors
{
    public class CartDoesNotExistsError : Error
    {
        public CartDoesNotExistsError(int customerId)
            :base($"The shopping cart does not exist for customer with id {customerId}")
        {
        }
    }
}
