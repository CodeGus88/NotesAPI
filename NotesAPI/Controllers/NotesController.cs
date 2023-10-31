using Microsoft.AspNetCore.Mvc;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.Services;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController: ControllerBase
    {
        private readonly INoteService service;

        public NotesController(INoteService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> Get() {
            return await service.GetAll();
        }

        [HttpGet("{id}", Name ="findByIdNote")]
        public async Task<ActionResult<Note>> Get(Guid id) {
            bool exists = await service.existsById(id);
            if (!exists) return NotFound();
            return await service.FindById(id);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> Post(NoteRequest request) {
            var note = await service.Add(request);
            return new CreatedAtRouteResult("findByIdNote", new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, NoteRequest request) {
            bool exists = await service.existsById(id);
            if (!exists) return NotFound();
            await service.Edit(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool exists = await service.existsById(id);
            if (!exists) return NotFound();
            await service.Delete(id);
            return NoContent();
        }

    }
}
