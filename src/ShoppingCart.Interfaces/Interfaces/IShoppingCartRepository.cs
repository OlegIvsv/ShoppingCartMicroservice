using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Interfaces.Interfaces;

public interface IShoppingCartRepository
{
    Task Add(Cart cart);
    Task<Cart?> FindByCustomer(Guid customerId);
    Task<IEnumerable<Cart>> All();
    Task<bool> Delete(Guid customerId);
    Task Update(Cart cart);
}