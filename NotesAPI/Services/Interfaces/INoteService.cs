using NotesAPI.DTOs;
using NotesAPI.Entities;

namespace NotesAPI.Services.Interfaces
{
    public interface INoteService
    {
        Task<Note> Add(NoteRequest request);
        Task<Note> FindById(Guid id, bool useCache);
        Task<List<Note>> GetAll(bool useCache);
        Task Delete(Guid id);
        Task Edit(Guid id, NoteRequest request);
        Task<bool> ExistsById(Guid id, bool useCache);

        // List
        Task<Note> Add2(NoteRequest request);
        Task<Note> FindById2(Guid id, bool useCache);
        Task<List<Note>> GetAll2(bool useCache);
        Task Delete2(Guid id);
        Task Edit2(Guid id, NoteRequest request);
        Task<bool> ExistsById2(Guid id, bool useCache);
    }
}
