using NotesAPI.Repositories;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Services
{
    public class CacheAdmin : ICacheAdmin
    {
        private readonly IRedisRepository redisRepository;

        public CacheAdmin(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }
        public async Task ClearCache()
        {
            await redisRepository.DeleteAll();
        }

        public async Task<bool> ExistsKeyInCache(string key)
        {
            return await redisRepository.ExistsKey(key);
        }

        public List<string> GetKeys(string partialKey)
        {
            return redisRepository.GetKeys(partialKey);
        }
    }
}
