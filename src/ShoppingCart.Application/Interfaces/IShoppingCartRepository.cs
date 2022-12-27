using ShoppingCart.Domain.Models;

namespace ShoppingCart.Infrastructure.DataAccess
{
    public interface IShoppingCartRepository
    {
        Task<Cart?> FindByCustomer(int customerId);
    }
}
