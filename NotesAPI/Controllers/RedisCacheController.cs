using Microsoft.AspNetCore.Mvc;
using NotesAPI.Services;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("api/redis")]
    public class RedisCacheController: ControllerBase
    {
        private readonly NoteCacheService redisService;

        public RedisCacheController(NoteCacheService redisService)
        {
            this.redisService = redisService;
        }

        //[HttpGet("{partialKey}")]
        //public List<string> Get(string partialKey = "*") { 
        //    return redisService.GetKeys(partialKey);
        //}

        //[HttpPost]
        //public void Post([FromQuery] Guid id, [FromBody] NoteRequest request)
        //{
        //    redisService.Add(key, value);
        //}

        //[HttpDelete("{key}")]
        //public void Delete(Guid key) {
        //    redisService.Delete(key);
        //}

        [HttpDelete("clear")]
        public ActionResult ClearCache() {
            redisService.ClearCache();
            return NoContent();
        }
    }
}
