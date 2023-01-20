using MongoDB.Bson.Serialization;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.BsonMapping;

internal class CartMapping
{
    public void RegisterMap()
    {
        BsonClassMap.RegisterClassMap<Cart>(map =>
        {
            map.MapField(cart => cart.Items).SetElementName("items");

            string factoryMethodName = nameof(Cart.Create);
            var methodInfo = typeof(Cart).GetMethod(factoryMethodName);
            map.MapFactoryMethod(methodInfo, "Id", "Items");
        });
    }
}