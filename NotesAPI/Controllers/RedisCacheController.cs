using Microsoft.AspNetCore.Mvc;
using NotesAPI.Services;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("api/redis")]
    public class RedisCacheController: ControllerBase
    {
        private readonly INoteCacheService redisService;

        public RedisCacheController(INoteCacheService redisService)
        {
            this.redisService = redisService;
        }

        [HttpGet("{partialKey}")]
        public ActionResult<List<string>> GetKeys(string partialKey = "*") {
            return redisService.GetKeys(partialKey);
        }

        [HttpDelete("clear")]
        public ActionResult ClearCache() {
            redisService.ClearCache();
            return NoContent();
        }
    }
}
