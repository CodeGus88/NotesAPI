using NotesAPI.DTOs;

namespace NotesAPI.Services
{
    public interface INoteService
    {
        Task<Note> Add(NoteRequest request);
        Task<Note> FindById(Guid id);
        Task<List<Note>> GetAll();
        Task Delete(Guid id);
        Task<Note> Edit(Guid id, NoteRequest request);
        Task<bool> existsById(Guid id);
    }
}
