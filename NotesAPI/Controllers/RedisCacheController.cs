using Microsoft.AspNetCore.Mvc;
using NotesAPI.Repositories;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("api/redis")]
    public class RedisCacheController: ControllerBase
    {
        private readonly RedisRepository redisRepository;

        public RedisCacheController(RedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }

        [HttpGet("{partialKey}")]
        public List<string> Get(string partialKey = "*") { 
            return redisRepository.GetKeys(partialKey);
        }

        [HttpPost()]
        public void Post([FromQuery] string key, [FromQuery] string value)
        {
            redisRepository.Add(key, value);
        }

        [HttpDelete("{key}")]
        public void Delete(string key) {
            redisRepository.Delete(key);
        }

        [HttpDelete]
        public void Delete() {
            redisRepository.DeleteAll();
        }
    }
}
