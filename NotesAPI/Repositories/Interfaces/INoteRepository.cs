//using NotesAPI.Entities;
using NotesAPI.Models;

namespace NotesAPI.Repositories.Interfaces
{
    public interface INoteRepository : IRepository<NoteEntity, Guid>, IRepositoryAync<NoteEntity, Guid>
    {
        // métodos de noteRepository
    }
}
