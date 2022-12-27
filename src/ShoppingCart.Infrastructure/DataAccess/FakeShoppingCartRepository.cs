using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.DataAccess
{
    internal class FakeShoppingCartRepository : IShoppingCartRepository
    {
        private static List<Cart> _carts;

        static FakeShoppingCartRepository()
        {
            _carts = new()
            {
                new Cart(
                    id : Guid.NewGuid().ToString(),
                    customerId : 1,
                    items : new List<CartItem>()
                    {
                        CartItem.Create(productId: 134, productTitle: "Prod_1",
                            unitPrice: 450, quantity: 2),
                        CartItem.Create(productId: 135, productTitle: "Prod_2",
                            unitPrice: 310, quantity: 1)
                    }),
               new Cart(
                    id : Guid.NewGuid().ToString(),
                    customerId : 2,
                    items : new List<CartItem>()
                    {
                         CartItem.Create(productId: 145, productTitle: "Prod_3",
                            unitPrice: 50, quantity: 7),
                         CartItem.Create(productId: 11, productTitle: "Prod_4",
                            unitPrice: 105, quantity: 1)
                    })
            };
        }
        public async Task<Cart?> FindByCustomer(int customerId)
        {
            return _carts
                .Where(cart =>  cart.CustomerId== customerId)
                .FirstOrDefault();
        }
    }
}
