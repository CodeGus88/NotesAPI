namespace NotesAPI.Repositories
{
    public interface IRedisRepository
    {
        bool Add<T>(string key, T value);
        bool Delete(string key);
        void DeleteAll();
        bool ExistsKey(string key);
        T FindByKey<T>(string key);
        List<string> GetKeys(string partialKey);
        List<T> GetAll<T>(string partialKey);
    }
}