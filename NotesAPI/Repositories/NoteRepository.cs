//using NotesAPI.Entities;
using NotesAPI.Models;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Utils;

namespace NotesAPI.Repositories
{
    public class NoteRepository : DapperRepository<NoteEntity, Guid>, INoteRepository {

        public NoteRepository(DbContext context): base(context)
        {
            
        }

    }
}
