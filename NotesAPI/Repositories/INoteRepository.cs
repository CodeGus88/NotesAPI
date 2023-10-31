using NotesAPI.DTOs;

namespace NotesAPI.Repositories
{
    public interface INoteRepository
    {
        Task<Guid> Add(NoteRequest request);
        Task<Note> FindById(Guid id);
        Task<List<Note>> GetAll();
        Task Delete(Guid id);
        Task Edit(Guid id, NoteRequest request);
        Task<bool> existsById(Guid id);
    }
}
