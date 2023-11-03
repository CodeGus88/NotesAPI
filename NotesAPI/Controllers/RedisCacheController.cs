using Microsoft.AspNetCore.Mvc;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("api/redis")]
    public class RedisCacheController: ControllerBase
    {
        private readonly ICacheAdmin cacheAdmin;

        public RedisCacheController(ICacheAdmin cacheAdmin)
        {
            this.cacheAdmin = cacheAdmin;
        }

        [HttpGet("{partialKey}")]
        public ActionResult<List<string>> GetKeys(string partialKey = "*") {
            return cacheAdmin.GetKeys(partialKey);
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearCache() {
            await cacheAdmin.ClearCache();
            return NoContent();
        }
    }
}
