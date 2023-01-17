﻿using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Interfaces.Interfaces;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IShoppingCartRepository _repository;

    public CartController(IShoppingCartRepository cartRepository)
    {
        _repository = cartRepository;
    }

    [HttpPost("{customerId}")]
    [ProducesResponseType(typeof(CartResponse), 201)]
    [ProducesResponseType(409)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateShoppingCart(Guid customerId)
    {
        var cartInRepo = await _repository.FindByCustomer(customerId);
        if (cartInRepo is not null)
            return Conflict();

        var cart = Cart.TryCreate(customerId);
        if (cart.IsFailed)
            return BadRequest();

        await _repository.Add(cart.Value);
        return CreatedAtAction(
            nameof(CreateShoppingCart),
            CartResponse.FromEntity(cart.Value));
    }

    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(CartResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetShoppingCart(Guid customerId)
    {
        if (customerId == Guid.Empty)
            return BadRequest();

        var cart = await _repository.FindByCustomer(customerId);
        if (cart is null)
            return NotFound();

        var responseCart = CartResponse.FromEntity(cart);
        return Ok(responseCart);
    }

    [HttpDelete("{customerId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteShoppingCart(Guid customerId)
    {
        if (customerId == Guid.Empty)
            return BadRequest();

        var deleted = await _repository.Delete(customerId);
        if (!deleted)
            return NotFound();

        return Ok();
    }

    [HttpPut("clear/{customerId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ClearShoppingCart(Guid customerId)
    {
        if (customerId == Guid.Empty)
            return BadRequest();

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
    public async Task<IActionResult> PutItemToCart(Guid customerId, [FromBody] CartItem? item)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

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
    public async Task<IActionResult> UpdateItemInCart(Guid customerId, CartItem item)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

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

        var deleted = cart.RemoveItem(productId);
        if (deleted)
            await _repository.Update(cart);
        return Ok();
    }
}