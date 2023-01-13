using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;

namespace ShoppingCart.Domain.Models
{
    public sealed class Cart : AggregateRoot<Cart>
    {
        private Dictionary<Guid, CartItem> _items;
        public IReadOnlyCollection<CartItem> Items => _items.Values;

        private Cart(Guid customerId)
        {
            base.Id = customerId;
            Id = customerId;
            _items= new Dictionary<Guid, CartItem>();
        }

        public static Result<Cart> Create(Guid customerId)
        {
            if (customerId == Guid.Empty)
                return Result.Fail(new InvalidIdValueError(customerId));
            return new Cart(customerId);
        }

        public void PutItem(CartItem item)
        {
            if (!ContainsItem(item))
                _items[item.ProductId] = item;
            else
                _items[item.ProductId].CorrectQuantityWith(item.ItemQuantity);
        }

        public void UpdateItem(CartItem item)
        {
            if(!ContainsItem(item))
                _items[item.ProductId] = item;
            else
                _items[item.ProductId].SetQuantity(item.ItemQuantity);
        }

        private bool ContainsItem(CartItem item)
        {
            if (item is null)
                throw new ArgumentNullException("Item can't be null here");
            return _items.ContainsKey(item.ProductId);
        }

        public bool RemoveItem(Guid productId)
            => _items.Remove(productId);

        public void Clear()
            => _items.Clear();

        public bool IsEmpty()
            => _items.Count == 0;
    }
}
