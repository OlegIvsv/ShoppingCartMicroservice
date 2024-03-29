﻿using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Interfaces.Interfaces;

public interface IShoppingCartRepository
{
    Task Save(Cart cart);
    Task<Cart?> FindByCustomer(Guid customerId);
    Task<bool> Delete(Guid customerId);
    Task<long> DeleteAbandoned(DateTime withoutUpdatesSince);
}