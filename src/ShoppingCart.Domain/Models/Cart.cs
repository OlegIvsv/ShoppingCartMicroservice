using ShoppingCart.Domain.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Models
{
    public sealed class Cart
    {
        private List<CartItem> _items;

        public string Id { get; private init; }
        public int CustomerId { get; private init; }
        public IReadOnlyList<CartItem> Items => _items;


        public Cart(string id, int customerId, List<CartItem> items)
        {
            Id = id;
            CustomerId = customerId;
            _items = items;
        }

        public Cart(int customerId)
        {
            CustomerId = customerId;
            _items = new List<CartItem>();
        }


        public bool PutItem(CartItem item)
        {
            var itemInCart = _items.Find(i => i.ProductId == item.ProductId);

            if (itemInCart is not null)
            {
                itemInCart.SetQuantity(item.Quantity);
                return false;
            }

            _items.Add(item);
            return true;
        }

        public void RemoveItem(int productId)
        {
            int indexOfItem = _items.FindIndex(item => productId == item.ProductId);
            if (indexOfItem < 0)
                throw new CartErrors.CartDoesNotContainItemException();
            
            _items.RemoveAt(indexOfItem);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool IsEmpty()
        {
            return _items.Count == 0;
        }
    }
}
