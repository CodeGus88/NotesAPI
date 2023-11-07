using NotesAPI.Entities;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Utils;

namespace NotesAPI.Repositories
{
    public class NoteRepository : DapperRepository<Note, Guid>, INoteRepository {

        public NoteRepository(DbContext context): base(context)
        {
            
        }

    }
}
