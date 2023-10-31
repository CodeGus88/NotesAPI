using NotesAPI.DTOs;
using NotesAPI.Repositories;

namespace NotesAPI.Services
{
    public class NoteService : INoteService
    {
        protected readonly INoteRepository dapperRepository;

        public NoteService(INoteRepository repository)
        {
            this.dapperRepository = repository;
        }
        public async Task<Note> Add(NoteRequest request)
        {
            Guid id = await dapperRepository.Add(request);
            return await dapperRepository.FindById(id);
        }

        public async Task Delete(Guid id)
        {
            await dapperRepository.Delete(id);
        }

        public async Task<Note> Edit(Guid id, NoteRequest request)
        {
            await dapperRepository.Edit(id, request);
            return await dapperRepository.FindById(id);
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
