using Microsoft.AspNetCore.Mvc;
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
