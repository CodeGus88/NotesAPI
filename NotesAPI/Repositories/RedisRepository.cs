using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace NotesAPI.Repositories
{
    public class RedisRepository : IRedisRepository
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

        public List<string> GetKeys(string partialKey)
        {
            var keys = redis.GetServer(config.GetConnectionString("RedisConnection")).Keys(pattern: partialKey);
            List<string> keyList = new List<string>();
            foreach (var key in keys)
            {
                keyList.Add(key);
            }
            return keyList;
        }

        public bool Add<T>(string key, T value)
        {
            return cacheDb.StringSet(
                key,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))
            );
        }

        public bool Delete(string key)
        {
            if (cacheDb.KeyExists(key)) cacheDb.KeyDelete(key);
            return false;
        }

        public void DeleteAll()
        {
            //var server = redis.GetServer(config.GetConnectionString("RedisConnection"), ",allowAdmin=true");
            //server.FlushAllDatabases();
            //var redis = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisConnection"));
            redis.GetDatabase().Execute("FLUSHDB");
        }

        public bool ExistsKey(string key)
        {
            return cacheDb.KeyExists(key);
        }

        public T FindByKey<T>(string key)
        {
            string element = cacheDb.StringGet(key);
            if (!element.IsNullOrEmpty())
                return JsonConvert.DeserializeObject<T>(element);
            return default;
        }

        public List<T> GetAll<T>(string partialKey)
        {
            var keys = redis.GetServer(config.GetConnectionString("RedisConnection")).Keys(pattern: partialKey);
            var values = cacheDb.StringGet(keys.ToArray());
            List<T> elements = values.Select(val => JsonConvert.DeserializeObject<T>(val)).ToList();
            return elements;
        }

    }
}
