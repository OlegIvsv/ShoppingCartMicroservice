using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.Serrializers
{

    public class CartItemsSerrializer : SerializerBase<Dictionary<Guid, CartItem>>
    {
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            Dictionary<Guid, CartItem> value)
        {
            ArraySerializer<CartItem> array = new ArraySerializer<CartItem>();
            array.Serialize(context, value.Values.ToArray());
        }

        public override Dictionary<Guid, CartItem> Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            var cartItemArray = BsonSerializer.Deserialize<CartItem[]>(context.Reader);
            return cartItemArray.ToDictionary(item => item.ProductId, item => item);
        }
    }
}
