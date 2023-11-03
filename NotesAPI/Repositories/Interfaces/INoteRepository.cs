using NotesAPI.Entities;

namespace NotesAPI.Repositories.Interfaces
{
    public interface INoteRepository : IRepository<Note, Guid>, IRepositoryAync<Note, Guid>
    {
        // métodos de noteRepository
    }
}
