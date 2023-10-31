using NotesAPI.DTOs;
using NotesAPI.Repositories;

namespace NotesAPI.Services
{
    public class NoteService : INoteService
    {
        protected readonly INoteRepository repository;

        public NoteService(INoteRepository repository)
        {
            this.repository = repository;
        }
        public virtual async Task<Note> Add(NoteRequest request)
        {
            Guid id = await repository.Add(request);
            return await repository.FindById(id);
        }

        public virtual async Task Delete(Guid id)
        {
            await repository.Delete(id);
        }

        public virtual async Task<Note> Edit(Guid id, NoteRequest request)
        {
            await repository.Edit(id, request);
            return await repository.FindById(id);
        }

        public virtual async Task<Note> FindById(Guid id)
        {
            return await repository.FindById(id);
        }

        public async Task<List<Note>> GetAll()
        {
            return await repository.GetAll();
        }

        public virtual async Task<bool> existsById(Guid id) {
            return await repository.existsById(id);
        }
    }
}
