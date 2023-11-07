using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NotesAPI.Repositories.Interfaces;
using StackExchange.Redis;
using System.Text;

namespace NotesAPI.Repositories
{
    public class RedisRepository : IRedisRepository/*, IRedisListRepository*/
    {
        private readonly IConfiguration config;
        private readonly IDatabase cacheDb;
        private readonly ConnectionMultiplexer redis;

        public RedisRepository(IConfiguration config)
        {
            this.config = config;
            redis = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisConnection"));
            cacheDb = redis.GetDatabase();
        }

        // REDIS ITEMS
        public List<string> GetKeys(string partialKey)
        {
            var server =  redis.GetServer(config.GetConnectionString("RedisConnection"));
            var keys = server.Keys(pattern: partialKey);
            List<string> keyList = new List<string>();
            foreach (var key in keys)
            {
                keyList.Add(key);
            }
            return keyList;
        }

        public async Task<bool> Set<T>(string key, T value)
        {
            bool success = await cacheDb.StringSetAsync(
                key,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))
            );
            return success;
        }

        public async Task<bool> Delete(string key)
        {
            if (cacheDb.KeyExists(key)) return await cacheDb.KeyDeleteAsync(key);
            return false;
        }

        public async Task DeleteAll()
        {
            await redis.GetDatabase().ExecuteAsync("FLUSHDB");
        }

        public async Task<bool> ExistsKey(string key)
        {
            return await cacheDb.KeyExistsAsync(key);
        }

        public async Task<T> FindByKey<T>(string key)
        {
            string element = await cacheDb.StringGetAsync(key);
            if (!element.IsNullOrEmpty())
                return JsonConvert.DeserializeObject<T>(element);
            return default;
        }

        public async Task<List<T>> GetAll<T>(string partialKey)
        {
            var keys = redis.GetServer(config.GetConnectionString("RedisConnection")).Keys(pattern: partialKey);
            var values = await cacheDb.StringGetAsync(keys.ToArray());
            List<T> elements = values.Select(val => JsonConvert.DeserializeObject<T>(val)).ToList();
            return elements;
        }

    }
}
