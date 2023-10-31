using Dapper;
using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using NotesAPI.Entities;
using NotesAPI.Utils;

namespace NotesAPI.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private SqlConnection connection;
        private readonly string table;

        public NoteRepository(DbContext context)
        {
            connection = context.Connection;
            table = "Notes";
        }
        public async Task Add(Note note)
        {
            //Guid guid = Guid.NewGuid();
            //string sqlInsert = $"INSERT INTO {table} (Id, Title, Content) VALUES ('{guid}', @Title, @Content)";
            //await connection.ExecuteAsync(sqlInsert, request);
            //return guid;
            await connection.InsertAsync(note);
        }

        public async Task Delete(Guid id)
        {
            //await connection.ExecuteAsync($"DELETE FROM {table} WHERE Id = @id", new { Id = id});
            await connection.DeleteAsync(new Note { Id = id});
        }

        public async Task Edit(Note note)
        {
            //var query = $"UPDATE {table} SET Title = @title, Content = @content WHERE Id = @id";
            //await connection.ExecuteAsync(query, new { Id = id, Title = request.Title, Content = request.Content});
            await connection.UpdateAsync(note);
        }

        public async Task<Note> FindById(Guid id)
        {
            //NoteDto note = await connection.QueryFirstOrDefaultAsync<NoteDto>($"SELECT * FROM {table} WHERE Id = @id", new { Id = id });
            //return note;
            return await connection.GetAsync(new Note { Id = id});
        }

        public async Task<List<Note>> GetAll()
        {
            //var notes = await connection.QueryAsync<NoteDto>($"SELECT * FROM {table} ORDER BY Title ASC", null);
            //return notes.ToList();
            return (await connection.FindAsync<Note>()).ToList();
        }

        public async Task<bool> existsById(Guid id) {
            string query = $"SELECT COUNT(*) FROM Notes WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<int>(query, new { Id = id }) > 0 ? true : false;
        }

    }
}
