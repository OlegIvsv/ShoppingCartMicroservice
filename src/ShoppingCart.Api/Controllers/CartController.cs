﻿using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;
using ShoppingCart.Infrastructure.DataAccess;

namespace ShoppingCart.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IShoppingCartRepository _repository;

        public CartController(IShoppingCartRepository cartRepository)
        {
            _repository = cartRepository;
        }


        [HttpPost("customerId")]
        [ProducesResponseType(typeof(CartResponse), 201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateShoppingCart(Guid customerId)
        {
            var cartInRepo = await _repository.FindByCustomer(customerId);
            if (cartInRepo is not null)
                return Conflict();

            var cart = Cart.Create(customerId);
            if (cart.IsFailed)
                return BadRequest();

            await _repository.Add(cart.Value);
            return CreatedAtAction(
                nameof(CreateShoppingCart),
                MapResponse(cart.Value));
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(CartResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetShoppingCart(Guid customerId)
        {
            var cart = await _repository.FindByCustomer(customerId);
            if (cart is null)
                return NotFound();

            var responseCart = MapResponse(cart);
            return Ok(responseCart);
        }

        [HttpDelete("{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteShoppingCart(Guid customerId)
        {
            bool deleted = await _repository.Delete(customerId);

            if (!deleted)
                return NotFound();

            return Ok();
        }

        [HttpPut("clear/{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ClearShoppingCart(Guid customerId)
        {
            var cart = await _repository.FindByCustomer(customerId);
            if (cart is null)
                return NotFound();

            cart.Clear();
            await _repository.Update(cart);
            return Ok();
        }

        [HttpPut("put-item/{customerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutItemToCart(Guid customerId, [FromBody] CartItemRequest request)
        {
            var item = MapRequest(request);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cart = await _repository.FindByCustomer(customerId);
            if (cart is null)
                return NotFound();

            cart.PutItem(item);
            await _repository.Update(cart);

            return Ok();
        }

        [HttpPut("update-item/{customerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateItemInCart(Guid customerId, [FromBody] CartItemRequest request)
        {
            var item = MapRequest(request);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cart = await _repository.FindByCustomer(customerId);
            if (cart is null)
                return NotFound();

            cart.UpdateItem(item);
            await _repository.Update(cart);

            return Ok();
        }

        [HttpPut("remove-item/{customerId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveItemFromCart(Guid customerId, [FromQuery] Guid productId)
        {
            var cart = await _repository.FindByCustomer(customerId);
            if (cart is null)
                return NotFound();

            bool deleted = cart.RemoveItem(productId);
            if (deleted)
                await _repository.Update(cart);

            return Ok();
        }


        private CartItem? MapRequest(CartItemRequest request)
        {
            var title = ProductTitle.Create(request.ProductTitle);
            var quantity = Quantity.Create(request.Quantity);
            var unitPrice = Money.Create(request.UnitPrice);
            var discount = Discount.Create(request.Discount);

            if (title.IsFailed)
                ModelState.AddModelError("ProductTitle", title.Errors.First().Message);
            if (quantity.IsFailed)
                ModelState.AddModelError("Quantity", title.Errors.First().Message);
            if (unitPrice.IsFailed)
                ModelState.AddModelError("UnitPrice", title.Errors.First().Message);
            if (discount.IsFailed)
                ModelState.AddModelError("Discount", title.Errors.First().Message);

            if (ModelState.ErrorCount != 0)
                return null;

            var item = CartItem.Create(
                request.ProductId,
                title.Value,
                quantity.Value,
                unitPrice.Value,
                discount.Value);

            if (item.IsFailed)
            {
                ModelState.AddModelError("CartItem", item.Errors.First().Message);
                return null;
            }

            return item.Value;
        }

        private CartItemResponse MapResponse(CartItem item)
        {
            return new CartItemResponse(
                item.Id,
                item.ProductId,
                item.UnitPrice.Value,
                item.ProductTitle.Value,
                item.Quantity.Value,
                item.Discount.Value);
        }

        private CartResponse MapResponse(Cart cart)
        {
            return new CartResponse(
                cart.Id,
                cart.Items.Select(item => MapResponse(item)).ToArray());
        }
    }
}