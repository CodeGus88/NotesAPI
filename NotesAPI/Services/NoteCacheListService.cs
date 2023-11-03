using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.EnumsAndStatics;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Services
{
    public class NoteCacheListService : NoteService, INoteCacheListService
    {
        private readonly IRedisRepository redisRepository;
        private readonly string key;
        public NoteCacheListService(INoteRepository noteRepository, IRedisRepository redisRepository, IMapper mapper): base(noteRepository, mapper)
        {
            this.redisRepository = redisRepository;
            key = PartialKey.NOTES;
            Console.WriteLine("Ejecutar NoteCacheListService....");
        }
        public override async Task<Note> Add(NoteRequest request)
        {
            Note note = await base.Add(request);

            List<Note> list;
            if (!await redisRepository.ExistsKey(key))
                list = new List<Note>();
            else
                list = await redisRepository.FindByKey<List<Note>>(key);
            list.Add(note);
            await redisRepository.Set(key, list);
            return note;
        }

        public override async Task Delete(Guid id)
        {
            await base.Delete(id);

            var list = await redisRepository.FindByKey<List<Note>>(key);
            if (list != null) list.RemoveAll(i => i.Id == id);
            else list = new List<Note>();
            await redisRepository.Set(key, list);
        }

        public override async Task Edit(Guid id, NoteRequest request)
        {
            await base.Edit(id, request);

            var list = await redisRepository.FindByKey<List<Note>>(key);
            if (list != null) {
                Note note = list.FirstOrDefault(i => i.Id == id);
                if (note != null) {
                    note.Title = request.Title;
                    note.Content = request.Content;
                }
            } 
            else list = new List<Note>();
            await redisRepository.Set(key, list);
        }

        public override async Task<bool> ExistsById(Guid id)
        {
            return await base.ExistsById(id);
        }

        public override async Task<Note> FindById(Guid id)
        {
            var list = await redisRepository.FindByKey<List<Note>>(key);
            Note note = list.FirstOrDefault(i => i.Id == id);
            return note;
        }

        public override async Task<List<Note>> GetAll()
        {
            var cache = await redisRepository.FindByKey<List<Note>>(key);
            if (cache == null || cache.Count() == 0)
                cache = await base.GetAll();
            await redisRepository.Set(key, cache);
            return cache;
        }
    }
}
