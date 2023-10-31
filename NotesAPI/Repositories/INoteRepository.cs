using NotesAPI.DTOs;
using NotesAPI.Entities;

namespace NotesAPI.Repositories
{
    public interface INoteRepository
    {
        Task Add(Note request);
        Task<Note> FindById(Guid id);
        Task<List<Note>> GetAll();
        Task Delete(Guid id);
        Task Edit(Note request);
        Task<bool> existsById(Guid id);
    }
}
