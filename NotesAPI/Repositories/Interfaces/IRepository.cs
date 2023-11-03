using NotesAPI.EnumsAndStatics;

namespace NotesAPI.Repositories.Interfaces
{
    public interface IRepository<TEntity, TId>
    {
        TEntity Get(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TId? Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        bool existsById(ETable table, TId id);
        void DeleteByIds(ETable table, List<TId> ids);
    }
}
