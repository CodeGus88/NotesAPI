using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.EnumsAndStatics;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Services
{
    public class NoteService : INoteService
    {
        protected readonly INoteRepository noteRepository;
        private readonly IMapper mapper;

        public NoteService(INoteRepository repository, IMapper mapper)
        {
            this.noteRepository = repository;
            this.mapper = mapper;
            Console.WriteLine("Ejecutar NoteService....");
        }
        public virtual async Task<Note> Add(NoteRequest request)
        {
            Note note = mapper.Map<Note>(request);
            note.Id = Guid.NewGuid();
            await noteRepository.InsertAsync(note);
            return note;
        }

        public virtual async Task Delete(Guid id)
        {
            await noteRepository.DeleteAsync(new Note { Id = id});
        }

        public virtual async Task Edit(Guid id, NoteRequest request)
        {
            Note note = mapper.Map<Note>(request);
            note.Id = id;
            await noteRepository.UpdateAsync(note);
        }

        public virtual async Task<Note> FindById(Guid id)
        {
            return await noteRepository.GetAsync(new Note { Id = id});
        }

        public virtual async Task<List<Note>> GetAll()
        {
            return (await noteRepository.GetAllAsync()).ToList();
        }

        public virtual async Task<bool> ExistsById(Guid id) {
            return await noteRepository.existsByIdAsync(ETable.Notes, id);
        }
    }
}
