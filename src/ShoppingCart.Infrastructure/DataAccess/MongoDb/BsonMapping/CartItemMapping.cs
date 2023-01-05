using MongoDB.Bson.Serialization;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.BsonMapping
{
    internal class CartItemMapping
    {
        public void RegisterMap()
        {
            BsonClassMap.RegisterClassMap<CartItem>(map =>
            {
                map.MapProperty(item => item.ProductId).SetElementName("productId");
                map.MapProperty(item => item.ProductTitle).SetElementName("productTitle");
                map.MapField(item => item.UnitPrice).SetElementName("unitPrice");
                map.MapField(item => item.Quantity).SetElementName("quantity");
                map.MapField(item => item.Discount).SetElementName("discount");
            });
        }
    }
}
