using MongoDB.Driver;
using Shop_TG.DAL.Configs;
using Shop_TG.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Repositories
{
    public class PaymentRepository
    {
        private readonly Config _config;

        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public PaymentRepository(Config config)
        {
            _config = config;

            _connectionString = _config.Mongo.ConnectionString;
            _databaseName = _config.Mongo.DatabaseName;
            _collectionName = "payments";
        }

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(_connectionString);
            var db = client.GetDatabase(_databaseName);
            return db.GetCollection<T>(collection);
        }

        public async Task Create(Payments item)
        {
            var collection = ConnectToMongo<Payments>(_collectionName);
            await collection.InsertOneAsync(item);
        }

        public async Task<Payments> GetById(int id)
        {
            var collection = ConnectToMongo<Payments>(_collectionName);
            return await collection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(Payments item)
        {
            var collection = ConnectToMongo<Payments>(_collectionName);

            var filter = Builders<Payments>.Filter.Eq(i => i.Id, item.Id);
            await collection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteById(int id)
        {
            var collection = ConnectToMongo<Payments>(_collectionName);

            var filter = Builders<Payments>.Filter.Eq(i => i.Id, id);
            await collection.DeleteOneAsync(filter);
        }
    }
}
