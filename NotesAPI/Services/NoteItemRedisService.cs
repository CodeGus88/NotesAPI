using Microsoft.OpenApi.Extensions;
using NotesAPI.DTOs;
using NotesAPI.ENUMs;
using NotesAPI.Repositories;

namespace NotesAPI.Services
{
    public class NoteItemRedisService: NoteService
    {
        
        private readonly IRedisRepository redisRepository;

        public NoteItemRedisService(INoteRepository noteRepository, IRedisRepository redisRepository): base(noteRepository)
        {
            this.redisRepository = redisRepository;
        }

        public async Task<Note> Add(NoteRequest request)
        {
            Note note = await base.Add(request);
            redisRepository.Add(
                $"{EPartialKey.NOTE.GetDisplayName()}{note.Id}", 
                note
            );
            return note;
        }

        public async Task Delete(Guid id)
        {
            await base.Delete(id);
            redisRepository.Delete($"{EPartialKey.NOTE.GetDisplayName()}{id}");
        }

        public void ClearCache()
        {
            redisRepository.DeleteAll();
        }

        public async Task<Note> Edit(Guid id, NoteRequest request)
        {
            Note note = await base.Edit(id, request);
            if(note != null)
            redisRepository.Add($"{EPartialKey.NOTE.GetDisplayName()}{id}", note);
            return note;
        }

        public async Task<Note> FindById(Guid id)
        {
            Note note = redisRepository.FindByKey<Note>($"{EPartialKey.NOTE.GetDisplayName()}{id}");
            if (note == null) {
                note = await dapperRepository.FindById(id);
                redisRepository.Add($"{EPartialKey.NOTE.GetDisplayName()}{id}", note);
            } 
            return note;
        }

        public async Task<List<Note>> GetAll()
        {
            List<Note> notes = redisRepository.GetAll<Note>($"{EPartialKey.NOTE.GetDisplayName()}*");
            if (notes.Count() == 0) {
                notes = await dapperRepository.GetAll();
                notes.ForEach(i => redisRepository.Add($"{EPartialKey.NOTE.GetDisplayName()}{i.Id}", i));
            }
            return notes;
        }
    }
}
