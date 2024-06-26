﻿using MongoDB.Driver;
using Shop_TG.DAL.Configs;
using Shop_TG.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Repositories
{
    public class ShopItemRepository
    {
        private readonly Config _config;

        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public ShopItemRepository(Config config)
        {
            _config = config;

            _connectionString = _config.Mongo.ConnectionString;
            _databaseName = _config.Mongo.DatabaseName;
            _collectionName = "shop-items";
        }

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(_connectionString);
            var db = client.GetDatabase(_databaseName);
            return db.GetCollection<T>(collection);
        }

        public async Task Create(ShopItem item)
        {
            var collection = ConnectToMongo<ShopItem>(_collectionName);
            await collection.InsertOneAsync(item);
        }

        public async Task<ShopItem> GetById(string id)
        {
            var collection = ConnectToMongo<ShopItem>(_collectionName);
            return await collection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ShopItem>> GetAll()
        {
            var collection = ConnectToMongo<ShopItem>(_collectionName);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task Update(ShopItem item)
        {
            var collection = ConnectToMongo<ShopItem>(_collectionName);

            var filter = Builders<ShopItem>.Filter.Eq(i => i.Id, item.Id);
            await collection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteById(string id)
        {
            var collection = ConnectToMongo<ShopItem>(_collectionName);

            var filter = Builders<ShopItem>.Filter.Eq(i => i.Id, id);
            await collection.DeleteOneAsync(filter);
        }
    }
}
