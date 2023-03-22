using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Api.Contracts.ContractAttributes;
using ShoppingCart.Api.Contracts.Swagger;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Interfaces.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace ShoppingCart.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Authorize(Policy = "owner-only")]
public class CartController : ControllerBase
{
    private readonly IShoppingCartRepository _repository;

    public CartController(IShoppingCartRepository cartRepository)
    {
        _repository = cartRepository;
    }

    [HttpPost("{customerId:guidID}")]
    [ProducesResponseType(typeof(CartResponse), 201)]
    [ProducesResponseType(409)]
    [ProducesResponseType(400)]
    [AllowAnonymous]
    public async Task<IActionResult> CreateShoppingCart(
        Guid customerId,
        [FromQuery] bool isAnonymous = true)
    {
        var cartInRepo = await _repository.FindByCustomer(customerId);
        if (cartInRepo is not null)
            return Conflict();

        var cart = Cart.TryCreate(customerId, isAnonymous);
        if (cart.IsFailed)
            return BadRequest();

        await _repository.Save(cart.Value);
        return CreatedAtAction(
            nameof(CreateShoppingCart),
            CartResponse.FromEntity(cart.Value));
    }

    [HttpGet("{customerId:guidID}")]
    [ProducesResponseType(typeof(CartResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetShoppingCart(Guid customerId)
    {
        var cart = await _repository.FindByCustomer(customerId);
        if (cart is null)
            return NotFound();

        var responseCart = CartResponse.FromEntity(cart);
        return Ok(responseCart);
    }

    [HttpDelete("{customerId:guidID}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteShoppingCart([GuidId] Guid customerId)
    {
        var deleted = await _repository.Delete(customerId);
        if (!deleted)
            return NotFound();

        return Ok();
    }

    [HttpPut("clear/{customerId:guidID}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ClearShoppingCart(Guid customerId)
    {
        var cart = await _repository.FindByCustomer(customerId);
        if (cart is null)
            return NotFound();

        cart.Clear();
        await _repository.Save(cart);
        return Ok();
    }

    [HttpPut("put-item/{customerId:guidID}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerRequestExample(typeof(CartItem), typeof(CartItemExampleProvider))]
    public async Task<IActionResult> PutItemToCart( 
        Guid customerId, 
        [FromBody] CartItem item)
    {
        var cart = await _repository.FindByCustomer(customerId);
        if (cart is null)
            return NotFound();

        cart.PutItem(item);
        await _repository.Save(cart);
        return Ok();
    }

    [HttpPut("update-item/{customerId:guidID}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerRequestExample(typeof(CartItem), typeof(CartItemExampleProvider))]
    public async Task<IActionResult> UpdateItemInCart(
        Guid customerId, 
        [FromBody] CartItem item)
    {
        var cart = await _repository.FindByCustomer(customerId);
        if (cart is null)
            return NotFound();

        cart.UpdateItem(item);
        await _repository.Save(cart);
        return Ok();
    }

    [HttpPut("remove-item/{customerId:guidID}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> RemoveItemFromCart(
        Guid customerId, 
        [FromQuery][GuidId] Guid productId)
    {
        var cart = await _repository.FindByCustomer(customerId);
        if (cart is null)
            return NotFound();

        var deleted = cart.RemoveItem(productId);
        if (deleted)
            await _repository.Save(cart);
        return Ok();
    }
}