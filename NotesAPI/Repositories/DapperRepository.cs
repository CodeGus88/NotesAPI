using Dapper;
using Dapper.FastCrud;
using NotesAPI.Entities;
using NotesAPI.EnumsAndStatics;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Utils;

namespace NotesAPI.Repositories
{
    public class DapperRepository<TEntity, TId> :
        IRepository<TEntity, TId>, IRepositoryAync<TEntity, TId> where TEntity : EntityBase<TId>
    {
        protected readonly DbContext context;

        public DapperRepository(DbContext context)
        {
            this.context = context;
        }

        #region Sync
        public virtual TEntity Get(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                return connection.Get(entity);
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> GetAll()
        {
            using (var connection = context.GetConnection())
            {
                return connection.Find<TEntity>();
            }
        }

        /// <inheritdoc />
        public virtual TId Insert(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                connection.Insert(entity);
                return entity.Id;
            }
        }

        /// <inheritdoc />
        public virtual void Update(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                connection.Update(entity);
            }
        }

        /// <inheritdoc />
        public virtual void Delete(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                connection.Delete(entity);
            }
        }

        public bool ExistsById(ETable table, TId id)
        {
            using (var connection = context.GetConnection())
            {
                string query = $"SELECT COUNT(*) FROM {table} WHERE Id = @Id";
                return connection.QueryFirstOrDefault<int>(query, new { Id = id }) > 0 ? true : false;
            }
        }

        public void DeleteByIds(ETable table, List<TId> ids)
        {
            string sql = $@"DELETE FROM {table} WHERE Id IN (@ids) AND OnSuccessTaskId IS NOT NULL
            DELETE FROM RestaurantTask WHERE Id IN (@ids) AND OnSuccessTaskId IS NULL";

            using (var connection = context.GetConnection())
            {
                connection.Execute(sql, new { ids });
            }
        }
        #endregion

        #region Async
        /// <inheritdoc />
        public virtual async Task<TEntity> GetAsync(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                return await connection.GetAsync(entity);
            }
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using (var connection = context.GetConnection())
            {
                return await connection.FindAsync<TEntity>();
            }
        }

        /// <inheritdoc />
        public virtual async Task<TId?> InsertAsync(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                await connection.InsertAsync(entity);
                return entity.Id;
            }
        }

        /// <inheritdoc />
        public virtual async Task UpdateAsync(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                await connection.UpdateAsync(entity);
            }
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(TEntity entity)
        {
            using (var connection = context.GetConnection())
            {
                await connection.DeleteAsync(entity);
            }
        }

        public virtual async Task<bool> ExistsByIdAsync(ETable table, TId id)
        {
            using (var connection = context.GetConnection())
            {
                string query = $"SELECT COUNT(*) FROM {table} WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<int>(query, new { Id = id }) > 0 ? true : false;
            }
        }

        public virtual async Task DeleteByIdsAsync(ETable table, List<TId> ids)
        {
            string sql = $@"DELETE FROM {table} WHERE Id IN (@Ids) AND OnSuccessTaskId IS NOT NULL
            DELETE FROM RestaurantTask WHERE Id IN (@Ids) AND OnSuccessTaskId IS NULL";

            using (var connection = context.GetConnection())
            {
                await connection.ExecuteAsync(sql, new { Ids = ids });
            }
        }
        #endregion

    }
}
