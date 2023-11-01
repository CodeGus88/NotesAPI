﻿using Microsoft.AspNetCore.Mvc;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.ENUMs;
using NotesAPI.Services;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService noteService;
        private readonly INoteCacheService noteCacheService;
        private INoteService optionService;

        public NotesController(INoteService noteService, INoteCacheService noteCacheService)
        {
            this.noteService = noteService;
            this.noteCacheService = noteCacheService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> Get([FromQuery] ECache cacheOption = ECache.WITH_ITEM_CACHE)
        {
            ConfigService(cacheOption);
            return await optionService.GetAll();
        }

        [HttpGet("{id}", Name = "findByIdNote")]
        public async Task<ActionResult<Note>> Get(Guid id, [FromQuery] ECache cacheOption = ECache.WITH_ITEM_CACHE)
        {
            ConfigService(cacheOption);
            bool exists = await noteService.existsById(id);
            if (!exists) return NotFound();
            return await optionService.FindById(id);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> Post(NoteRequest request, [FromQuery] ECache cacheOption = ECache.WITH_ITEM_CACHE)
        {
            ConfigService(cacheOption);
            var note = await optionService.Add(request);
            return new CreatedAtRouteResult("findByIdNote", new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, NoteRequest request, [FromQuery] ECache cacheOption = ECache.WITH_ITEM_CACHE)
        {
            ConfigService(cacheOption);
            bool exists = await noteCacheService.existsById(id);
            if (!exists) return NotFound();
            await optionService.Edit(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, [FromQuery] ECache cacheOption = ECache.WITH_ITEM_CACHE)
        {
            ConfigService(cacheOption);
            bool exists = await noteCacheService.existsById(id);
            if (!exists) return NotFound();
            await optionService.Delete(id);
            return NoContent();
        }

        private void ConfigService(ECache option) { 
            switch (option)
            {
                case ECache.WITHOUT_CACHE:
                    optionService = noteService; 
                    break;
                case ECache.WITH_ITEM_CACHE:
                    optionService = noteCacheService;
                    break;
                case ECache.WITH_LIST_CACHE:
                    throw new Exception("not implemented");

            }
        }


        //[HttpGet]
        //public async Task<ActionResult<List<Note>>> Get() {
        //    return await service.GetAll();
        //}

        //[HttpGet("{id}", Name ="findByIdNote")]
        //public async Task<ActionResult<Note>> Get(Guid id) {
        //    bool exists = await service.existsById(id);
        //    if (!exists) return NotFound();
        //    return await service.FindById(id);
        //}

        //[HttpPost]
        //public async Task<ActionResult<Note>> Post(NoteRequest request) {
        //    var note = await service.Add(request);
        //    return new CreatedAtRouteResult("findByIdNote", new { id = note.Id }, note);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put(Guid id, NoteRequest request) {
        //    bool exists = await service.existsById(id);
        //    if (!exists) return NotFound();
        //    await service.Edit(id, request);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    bool exists = await service.existsById(id);
        //    if (!exists) return NotFound();
        //    await service.Delete(id);
        //    return NoContent();
        //}

    }
}
