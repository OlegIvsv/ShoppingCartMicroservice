using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;
using ShoppingCart.Domain.ValueObjects;

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
            if (item is null)
                throw new ArgumentNullException("Item can't be null here");

            var itemInCart = _items.GetValueOrDefault(item.ProductId);

            if (itemInCart is null)
                _items[item.ProductId] = item;
            else
            {
                Quantity newQuantity = Quantity.Add(itemInCart.Quantity, item.Quantity);
                _items[item.ProductId].SetQuantity(newQuantity);
            }
        }

        public void UpdateItem(CartItem item)
        {
            if(item is null)
                throw new ArgumentNullException("Item can't be null here");
                
            var itemToUpdate = _items.GetValueOrDefault(item.ProductId);

            if(itemToUpdate is null)
                _items[item.ProductId] = item;
            else
                itemToUpdate.SetQuantity(item.Quantity);
        }

        public bool RemoveItem(Guid productId)
        {
            return _items.Remove(productId);
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
