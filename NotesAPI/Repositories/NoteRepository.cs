using Dapper;
using Microsoft.Data.SqlClient;
using NotesAPI.DTOs;
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
        public async Task<Guid> Add(NoteRequest request)
        {
                Guid guid = Guid.NewGuid();
                string sqlInsert = $"INSERT INTO {table} (Id, Title, Content) VALUES ('{guid}', @Title, @Content)";
                await connection.ExecuteAsync(sqlInsert, request);
                return guid;
        }

        public async Task Delete(Guid id)
        {
            await connection.ExecuteAsync($"DELETE FROM {table} WHERE Id = @id", new { Id = id});
        }

        public async Task Edit(Guid id, NoteRequest request)
        {
            var query = $"UPDATE {table} SET Title = @title, Content = @content WHERE Id = @id";
            await connection.ExecuteAsync(query, new { Id = id, Title = request.Title, Content = request.Content});
        }

        public async Task<Note> FindById(Guid id)
        {
            Note note = await connection.QueryFirstOrDefaultAsync<Note>($"SELECT * FROM {table} WHERE Id = @id", new { Id = id });
            return note;
        }

        public async Task<List<Note>> GetAll()
        {
            var notes = await connection.QueryAsync<Note>($"SELECT * FROM {table} ORDER BY Title ASC", null);
            return notes.ToList();
        }

        public async Task<bool> existsById(Guid id) {
            string query = $"SELECT COUNT(*) FROM Notes WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<int>(query, new { Id = id }) > 0?true:false;
        }
    }
}
