using Microsoft.AspNetCore.Mvc;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.EnumsAndStatics;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService noteService;
        private ECache cacheOption;

        public NotesController(
            INoteService noteService
        )
        {
            this.noteService = noteService;
            cacheOption = ECache.WITH_CACHE_ITEM;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> Get([FromQuery] bool useCache = true)
        {
            switch (cacheOption)
            {
                case ECache.WITH_CACHE_ITEM:
                    return await noteService.GetAll(useCache);
                case ECache.WITH_CACHE_LIST:
                    return await noteService.GetAll2(useCache);
                default:
                    return BadRequest();
            }
        }

        [HttpGet("{id}", Name = "findByIdNote")]
        public async Task<ActionResult<Note>> Get(
            Guid id,
            [FromQuery] bool useCache = true)
        {
            switch (cacheOption) 
            {
                case ECache.WITH_CACHE_ITEM:
                    bool exists = await noteService.ExistsById(id, useCache);
                    if (!exists) return NotFound();
                    return await noteService.FindById(id, useCache);

                case ECache.WITH_CACHE_LIST:
                    bool exists2 = await noteService.ExistsById2(id, useCache);
                    if (!exists2) return NotFound();
                    return await noteService.FindById2(id, useCache);

                default:
                    return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Note>> Post(NoteRequest request)
        {
            switch (cacheOption)
            {
                case ECache.WITH_CACHE_ITEM:
                    var note1 = await noteService.Add(request);
                    return new CreatedAtRouteResult("findByIdNote", new { id = note1.Id }, note1);

                case ECache.WITH_CACHE_LIST:
                    var note2 = await noteService.Add2(request);
                    return new CreatedAtRouteResult("findByIdNote", new { id = note2.Id }, note2);

                default:
                    return BadRequest();
            }
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, NoteRequest request)
        {
            switch (cacheOption)
            {
                case ECache.WITH_CACHE_ITEM:
                    bool exists = await noteService.ExistsById(id, true);
                    if (!exists) return NotFound();
                    await noteService.Edit(id, request);
                    return NoContent();

                case ECache.WITH_CACHE_LIST:
                    bool exists2 = await noteService.ExistsById2(id, true);
                    if (!exists2) return NotFound();
                    await noteService.Edit2(id, request);
                    return NoContent();

                default:
                    return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            switch (cacheOption)
            {
                case ECache.WITH_CACHE_ITEM:
                    bool exists = await noteService.ExistsById(id, true);
                    if (!exists) return NotFound();
                    await noteService.Delete(id);
                    return NoContent();

                case ECache.WITH_CACHE_LIST:
                    bool exists2 = await noteService.ExistsById2(id, true);
                    if (!exists2) return NotFound();
                    await noteService.Delete2(id);
                    return NoContent();
                default:
                    return BadRequest();
            }

        }

    }
}
