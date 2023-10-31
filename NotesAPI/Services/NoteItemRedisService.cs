using Microsoft.OpenApi.Extensions;
using NotesAPI.DTOs;
using NotesAPI.ENUMs;
using NotesAPI.Repositories;

namespace NotesAPI.Services
{
    public class NoteItemRedisService: NoteService
    {
        
        private readonly RedisRepository redisRep;

        public NoteItemRedisService(INoteRepository noteRep, RedisRepository redisRep): base(noteRep)
        {
            this.redisRep = redisRep;
        }

        public override async Task<Note> Add(NoteRequest request)
        {
            Note note = await base.Add(request);
            redisRep.Add(
                $"{EPartialKey.ITEM.GetDisplayName()}{note.Id}", 
                note
            );
            return note;
        }

        public override async Task Delete(Guid id)
        {
            await base.Delete(id);
            redisRep.Delete($"{EPartialKey.ITEM.GetDisplayName()}{id}");
        }

        public override async Task<Note> Edit(Guid id, NoteRequest request)
        {
            Note note = await base.Edit(id, request);
            if(note != null)
            redisRep.Add($"{EPartialKey.ITEM.GetDisplayName()}{id}", note);
            return note;
        }

        public override async Task<Note> FindById(Guid id)
        {
            Note note = redisRep.FindByKey<Note>($"{EPartialKey.ITEM.GetDisplayName()}{id}");
            if (note == null) {
                note = await repository.FindById(id);
                redisRep.Add($"{EPartialKey.ITEM.GetDisplayName()}{id}", note);
            } 
            return note;
        }

        public override async Task<List<Note>> GetAll()
        {
            List<Note> notes = redisRep.
            return await repository.GetAll();
        }

        public override async Task<bool> existsById(Guid id)
        {
            return await repository.existsById(id);
        }
    }
}
