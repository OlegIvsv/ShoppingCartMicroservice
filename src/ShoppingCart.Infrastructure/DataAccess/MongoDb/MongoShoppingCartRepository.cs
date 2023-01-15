using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;
using ShoppingCart.Infrastructure.DataAccess.MongoDb.BsonMapping;
using ShoppingCart.Infrastructure.DataAccess.MongoDb.Serrializers;

namespace ShoppingCart.Infrastructure.DataAccess
{
    public class MongoShoppingCartRepository : IShoppingCartRepository
    {
        private MongoClient _client;
        private IMongoDatabase _shopDb;
        private IMongoCollection<Cart> _cartsCollection;


        public MongoShoppingCartRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _shopDb = _client.GetDatabase(settings.Value.Database);
            _cartsCollection = _shopDb.GetCollection<Cart>(settings.Value.ShoppingCartsCollection);
        }

        static MongoShoppingCartRepository()
        {
            BsonSerializer.RegisterSerializer<Money>(new MoneySerrializer());
            BsonSerializer.RegisterSerializer<Quantity>(new QuantitySerrializer());
            BsonSerializer.RegisterSerializer<Discount>(new DiscountSerrializer());
            BsonSerializer.RegisterSerializer<ProductTitle>(new ProductTitleSerrializer());

            new CartItemMapping().RegisterMap();
            new CartMapping().RegisterMap();
        }


        public async Task Add(Cart cart)
        {
            await _cartsCollection.InsertOneAsync(cart);
        }

        public async Task<Cart?> FindByCustomer(Guid customerId)
        {
            var cart = await _cartsCollection
                .Find(cart => cart.Id == customerId)
                .FirstOrDefaultAsync();
            return cart;
        }

        public async Task Update(Cart cart)
        {
            await _cartsCollection.ReplaceOneAsync(c => c.Id == cart.Id, cart);
        }

        public async Task<IEnumerable<Cart>> All()
        {
            return await _cartsCollection
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<bool> Delete(Guid customerId)
        {
            var deleteResult = await _cartsCollection.DeleteOneAsync(cart => cart.Id == customerId);
            return deleteResult.DeletedCount > 0;
        }
    }
}
