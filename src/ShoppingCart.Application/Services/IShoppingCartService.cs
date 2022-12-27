using FluentResults;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Services
{
    public interface IShoppingCartService
    {
        Task<Result<Cart>> GetCartByCustomer(int customerId);
    }
}
