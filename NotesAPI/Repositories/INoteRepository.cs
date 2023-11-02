using NotesAPI.DTOs;
using NotesAPI.Entities;
using NotesAPI.Repositories.Dapper;

namespace NotesAPI.Repositories
{
    public interface INoteRepository: IRepository<Note, Guid>, IRepositoryAync<Note, Guid>
    {
        // métodos de noteRepository
    }
}
