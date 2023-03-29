using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;

namespace ShoppingCart.Domain.Entities;

public sealed class Cart : AggregateRoot<Cart>
{
    private readonly Dictionary<Guid, CartItem> _items;
    public IReadOnlyCollection<CartItem> Items => _items.Values;
    public bool IsAnonymous { get; private set; }
    public DateTime LastModifiedDate { get; private set; }

    private Cart(
        Guid customerId, 
        bool isAnonymous, 
        IEnumerable<CartItem> items)
    {
        Id = customerId;
        IsAnonymous = isAnonymous;
        LastModifiedDate = DateTime.Now;
        _items = new Dictionary<Guid, CartItem>();
        Fill(items);
    }

    public static Result<Cart> TryCreate(
        Guid customerId,
        bool isAnonymous = true,
        IEnumerable<CartItem>? items = null)
    {
        if (customerId == Guid.Empty)
            return Result.Fail(new InvalidIdValueError(customerId));
        return new Cart(customerId, isAnonymous, items);
    }

    public static Cart Create( 
        Guid customerId,
        bool isAnonymous = true,
        IEnumerable<CartItem>? items = null)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Invalid id value!", nameof(customerId));
        return new Cart(customerId, isAnonymous, items);
    }

    private void Fill(IEnumerable<CartItem>? items)
    {
        if (items is null)
            return;
        foreach (var item in items)
            PutItem(item);
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
        if (!ContainsItem(item))
            _items[item.ProductId] = item;
        else
            _items[item.ProductId].SetQuantity(item.ItemQuantity);
    }

    private bool ContainsItem(CartItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        return _items.ContainsKey(item.ProductId);
    }

    public bool RemoveItem(Guid productId)
        => _items.Remove(productId);

    public void Clear()
    {
        _items.Clear();
    }

    public bool IsEmpty()
        => _items.Count == 0;
}