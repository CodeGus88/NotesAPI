using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.EnumsAndStatics;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Services
{
    public class NoteCacheService: NoteService, INoteCacheService
    {
        //private readonly INoteRepository noteRepository;
        private readonly IMapper mapper;
        private readonly IRedisRepository redisRepository;

        public NoteCacheService(INoteRepository noteRepository, IRedisRepository redisRepository, IMapper mapper): base(noteRepository, mapper)
        {
            //this.noteRepository = noteRepository;
            this.mapper = mapper;
            this.redisRepository = redisRepository;
            Console.WriteLine("Ejecutar NoteCacheService....");
        }

        public override async Task<Note> Add(NoteRequest request)
        {
            Note note = await base.Add(request);
            await redisRepository.Set(
                $"{PartialKey.NOTE}{note.Id}",
                note
            );
            return note;
        }

        public override async Task Delete(Guid id)
        {
            await base.Delete(id);
            await redisRepository.Delete($"{PartialKey.NOTE}{id}");
        }

        public override async Task Edit(Guid id, NoteRequest request)
        {
            await base.Edit(id, request);
            Note note = mapper.Map<Note>(request);
            note.Id = id;
            await redisRepository.Set($"{PartialKey.NOTE}{id}", note);
        }

        public override async Task<Note> FindById(Guid id)
        {
            Note note = await redisRepository.FindByKey<Note>($"{PartialKey.NOTE}{id}");
            if (note == null) {
                note = await noteRepository.GetAsync(new Note { Id = id});
                await redisRepository.Set($"{PartialKey.NOTE}{id}", note);
            } 
            return note;
        }

        public override async Task<List<Note>> GetAll()
        {
            List<Note> notes = await redisRepository.GetAll<Note>($"{PartialKey.NOTE}*");
            if (notes.Count() == 0) {
                notes = (await noteRepository.GetAllAsync()).ToList();
                notes.ForEach(i => redisRepository.Set($"{PartialKey.NOTE}{i.Id}", i));
            }
            return notes;
        }
    }
}
