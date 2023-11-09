using NotesAPI.DTOs;
using NotesAPI.Models;

namespace NotesAPI.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteEntity> Add(NoteRequest request);
        Task<NoteEntity> FindById(Guid id, bool useCache);
        Task<List<NoteEntity>> GetAll(bool useCache);
        Task Delete(Guid id);
        Task Edit(Guid id, NoteRequest request);
        Task<bool> ExistsById(Guid id, bool useCache);

        // List
        Task<NoteEntity> Add2(NoteRequest request);
        Task<NoteEntity> FindById2(Guid id, bool useCache);
        Task<List<NoteEntity>> GetAll2(bool useCache);
        Task Delete2(Guid id);
        Task Edit2(Guid id, NoteRequest request);
        Task<bool> ExistsById2(Guid id, bool useCache);
    }
}
