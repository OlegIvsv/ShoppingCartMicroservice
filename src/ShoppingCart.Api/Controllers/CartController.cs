﻿using FluentResults;
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
        private readonly IMapper _mapper;


        public CartController(IShoppingCartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }


        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetShoppingCartByCustomerId(int customerId)
        {
            var cart = await _cartService.GetCartByCustomer(customerId);
            if (cart.IsSuccess)
            {
                var responseCart = _mapper.Map<CartResponse>(cart.Value);
                return Ok(responseCart);
            }

            return Problem(cart.Errors);
        }

        [HttpPost("customerId")]
        public async Task<IActionResult> CreateShoppingCart(int customerId)
        {
            var createResult = await _cartService.CreateCart(customerId);
           
            if (createResult.IsSuccess)
            {
                var response = _mapper.Map<CartResponse>(createResult.Value);
                return CreatedAtAction(nameof(CreateShoppingCart), response);
            }

            return Problem(createResult.Errors);
        }

        private IActionResult Problem(IList<IError> errors)
        {
            var firstError = errors.First();

            var statusCode = firstError switch
            {
                CartDoesNotExistsError => StatusCodes.Status404NotFound,
                CartAlreadyExistsError => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemResult = Problem(statusCode: statusCode, detail: firstError.Message);
            return problemResult;
        }
    }
}
