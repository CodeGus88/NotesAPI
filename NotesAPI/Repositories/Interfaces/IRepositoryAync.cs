using NotesAPI.EnumsAndStatics;

namespace NotesAPI.Repositories.Interfaces
{
    public interface IRepositoryAync<TEntity, TId>
    {
        Task<TEntity> GetAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TId?> InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<bool> existsByIdAsync(ETable table, TId id);
        Task DeleteByIdsAsync(ETable table, List<TId> ids);
    }
}
