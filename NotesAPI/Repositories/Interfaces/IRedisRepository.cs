namespace NotesAPI.Repositories.Interfaces
{
    public interface IRedisRepository
    {
        Task<bool> Set<T>(string key, T value);
        Task<bool> Delete(string key);
        Task DeleteAll();
        Task<bool> ExistsKey(string key);
        Task<T> FindByKey<T>(string key);
        List<string> GetKeys(string partialKey);
        Task<List<T>> GetAll<T>(string partialKey);
    }
}