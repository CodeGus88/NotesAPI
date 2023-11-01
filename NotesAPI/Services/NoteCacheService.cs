using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.ENUMs;
using NotesAPI.Repositories;

namespace NotesAPI.Services
{
    public class NoteCacheService: INoteCacheService
    {
        private readonly INoteRepository noteRepository;
        private readonly IMapper mapper;
        private readonly IRedisRepository redisRepository;

        public NoteCacheService(INoteRepository noteRepository, IRedisRepository redisRepository, IMapper mapper)
        {
            this.noteRepository = noteRepository;
            this.mapper = mapper;
            this.redisRepository = redisRepository;
            Console.WriteLine("Ejecutar NoteCacheService....");
        }

        public async Task<Note> Add(NoteRequest request)
        {
            Note note = mapper.Map<Note>(request);
            note.Id = Guid.NewGuid();
            await noteRepository.Add(note);
            redisRepository.Add(
                $"{PartialKey.NOTE}{note.Id}",
                note
            );
            return note;
        }

        public async Task Delete(Guid id)
        {
            await noteRepository.Delete(id);
            redisRepository.Delete($"{PartialKey.NOTE}{id}");
        }

        public void ClearCache()
        {
            redisRepository.DeleteAll();
        }

        public async Task Edit(Guid id, NoteRequest request)
        {
            Note note = mapper.Map<Note>(request);
            note.Id = id;
            await noteRepository.Edit(note);
            redisRepository.Add($"{PartialKey.NOTE}{id}", note);
        }

        public async Task<Note> FindById(Guid id)
        {
            Note note = redisRepository.FindByKey<Note>($"{PartialKey.NOTE}{id}");
            if (note == null) {
                note = await noteRepository.FindById(id);
                redisRepository.Add($"{PartialKey.NOTE}{id}", note);
            } 
            return note;
        }

        public async Task<List<Note>> GetAll()
        {
            List<Note> notes = redisRepository.GetAll<Note>($"{PartialKey.NOTE}*");
            if (notes.Count() == 0) {
                notes = await noteRepository.GetAll();
                notes.ForEach(i => redisRepository.Add($"{PartialKey.NOTE}{i.Id}", i));
            }
            return notes;
        }

        public async Task<bool> existsById(Guid id)
        {
            return await noteRepository.existsById(id);
        }

        public bool ExistsKeyInCache(string key)
        {
            return redisRepository.ExistsKey(key);
        }

        public List<string> GetKeys(string partialKey)
        {
            return redisRepository.GetKeys(partialKey);
        }
    }
}
