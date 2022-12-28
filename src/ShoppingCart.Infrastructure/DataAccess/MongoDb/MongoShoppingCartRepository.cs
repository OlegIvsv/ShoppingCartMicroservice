using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using ShoppingCart.Domain.Models;
using System;

namespace ShoppingCart.Infrastructure.DataAccess
{
    public class MongoShoppingCartRepository : IShoppingCartRepository
    {
        private MongoClient _client;
        private IMongoDatabase _shopDb;
        private IMongoCollection<Cart> _cartsCollection;

        public MongoShoppingCartRepository(IOptions<MongoSettings> settings)
        {
            string connectionString = settings.Value.ConnectionString;
            string dbName = settings.Value.Database;
            string cartsCollectionName = settings.Value.ShoppingCartsCollection;

            _client = new MongoClient(connectionString);
            _shopDb = _client.GetDatabase(dbName);
            _cartsCollection = _shopDb.GetCollection<Cart>(cartsCollectionName);
        }

        static MongoShoppingCartRepository()
        {
            BsonClassMap.RegisterClassMap<CartItem>(initializer =>
            {
                initializer.MapProperty(item => item.ProductId)
                    .SetElementName("productId");
                initializer.MapField(item => item.ProductTitle)
                    .SetElementName("productTitle");
                initializer.MapField(item => item.UnitPrice)
                    .SetElementName("unitPrice");
                initializer.MapField(item => item.Quantity)
                    .SetElementName("quantity");
            });

            BsonClassMap.RegisterClassMap<Cart>(initializer =>
            {
                initializer.MapProperty(cart => cart.CustomerId)
                    .SetElementName("customerId");
                initializer.MapField("_items")
                    .SetElementName("items");
                initializer.MapIdProperty(c => c.Id)
                   .SetIdGenerator(new StringObjectIdGenerator())
                   .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }

        public async Task<Cart> Add(Cart cart)
        {
            await _cartsCollection.InsertOneAsync(cart);
            return cart;
        }

        public async Task<Cart?> FindByCustomer(int customerId)
        {
            var cart = await _cartsCollection
                .Find(cart => cart.CustomerId == customerId)
                .FirstOrDefaultAsync();
            return cart;
        }
    }
}
