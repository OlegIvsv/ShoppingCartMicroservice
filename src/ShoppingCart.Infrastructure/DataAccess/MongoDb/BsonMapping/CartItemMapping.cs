using MongoDB.Bson.Serialization;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.BsonMapping;

internal class CartItemMapping
{
    public void RegisterMap()
    {
        BsonClassMap.RegisterClassMap<CartItem>(map =>
        {
            map.MapProperty(item => item.ProductId).SetElementName("productId");
            map.MapProperty(item => item.ProductTitle).SetElementName("productTitle");
            map.MapField(item => item.UnitPrice).SetElementName("unitPrice");
            map.MapField(item => item.ItemQuantity).SetElementName("quantity");
            map.MapField(item => item.Discount).SetElementName("discount");

            string factoryMethodName = nameof(Cart.Create);
            var methodInfo = typeof(CartItem).GetMethod(factoryMethodName);
            map.MapFactoryMethod(
                methodInfo,
                "ProductId",
                "ProductTitle",
                "ItemQuantity",
                "UnitPrice",
                "Discount");
        });
    }
}