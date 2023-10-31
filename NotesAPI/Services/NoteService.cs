using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.Repositories;

namespace NotesAPI.Services
{
    public class NoteService : INoteService
    {
        protected readonly INoteRepository dapperRepository;
        private readonly IMapper mapper;

        public NoteService(INoteRepository repository, IMapper mapper)
        {
            this.dapperRepository = repository;
            this.mapper = mapper;
        }
        public async Task<Note> Add(NoteRequest request)
        {
            Note note = mapper.Map<Note>(request);
            note.Id = Guid.NewGuid();
            await dapperRepository.Add(note);
            return note;
        }

        public async Task Delete(Guid id)
        {
            await dapperRepository.Delete(id);
        }

        public async Task Edit(Guid id, NoteRequest request)
        {
            Note note = mapper.Map<Note>(request);
            note.Id = id;
            await dapperRepository.Edit(note);
        }

        public async Task<Note> FindById(Guid id)
        {
            return await dapperRepository.FindById(id);
        }

        public async Task<List<Note>> GetAll()
        {
            return await dapperRepository.GetAll();
        }

        public async Task<bool> existsById(Guid id) {
            return await dapperRepository.existsById(id);
        }
    }
}
