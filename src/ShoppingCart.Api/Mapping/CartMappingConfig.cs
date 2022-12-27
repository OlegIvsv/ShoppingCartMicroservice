using Mapster;
using ShoppingCart.Api.Contracts;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Api.Mapping
{
    public class CartMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CartItem, CartItemResponse>();
            config.NewConfig<Cart, CartItemResponse>();
        }
    }
}
