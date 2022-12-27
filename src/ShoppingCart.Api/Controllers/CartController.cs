using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Application.Errors;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;

        public CartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }


        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetShoppingCartByCustomerId(int customerId)
        {
            var cart = await _cartService.GetCartByCustomer(customerId);
            if (cart.IsSuccess)
            {
                var responseCart = MapToResponse(cart.Value);
                return Ok(responseCart);
            }

            return Problem(cart.Errors);
        }

        private IActionResult Problem(IList<IError> errors)
        {
            var firstError = errors.First();

            var statusCode = firstError switch
            {
                CartDoesNotExistsError => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemResult = Problem(statusCode: statusCode, detail: firstError.Message);
            return problemResult;
        }

        private CartResponse MapToResponse(Cart cart)
        {
            return new CartResponse(
                cart.Id,
                cart.CustomerId,
                cart.Items.Select(
                    item => new CartItemResponse(
                        item.ProductId,
                        item.UnitPrice,
                        item.ProductTitle,
                        item.Quantity)
                    ).ToList()
                );
        }
    }
}
