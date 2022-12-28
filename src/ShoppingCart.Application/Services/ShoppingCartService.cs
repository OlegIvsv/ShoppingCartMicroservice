using FluentResults;
using ShoppingCart.Application.Errors;
using ShoppingCart.Domain.Models;
using ShoppingCart.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private IShoppingCartRepository _cartRepository;

        public ShoppingCartService(IShoppingCartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Result<Cart>> GetCartByCustomer(int customerId)
        {
            var cart = await _cartRepository.FindByCustomer(customerId);
            if (cart is null)
                return Result.Fail(new CartDoesNotExistsError(customerId));

            return cart;
        }

        public async Task<Result<Cart>> CreateCart(int customerId)
        {
            var cart = await _cartRepository.FindByCustomer(customerId);
            if (cart is not null)
                return Result.Fail(new CartAlreadyExistsError(customerId));

            var newCart = new Cart(customerId);
            return await _cartRepository.Add(newCart);
        }

    }
}
