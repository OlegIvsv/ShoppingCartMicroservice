using MongoDB.Bson.Serialization;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.BsonMapping;

internal class CartMapping
{
    public void RegisterMap()
    {
        BsonClassMap.RegisterClassMap<Cart>(map =>
        {
            map.MapProperty(cart => cart.Items).SetElementName("items");
            map.MapProperty(cart => cart.IsAnonymous).SetElementName("isAnonymous").SetDefaultValue(false);
            map.MapProperty(cart => cart.LastModifiedDate).SetElementName("lastModifiedDate");

            string factoryMethodName = nameof(Cart.Create);
            var methodInfo = typeof(Cart).GetMethod(factoryMethodName);
            map.MapFactoryMethod(methodInfo, "Id", "IsAnonymous", "Items");
        });
    }
}