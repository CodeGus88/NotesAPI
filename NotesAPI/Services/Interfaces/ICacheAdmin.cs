namespace NotesAPI.Services.Interfaces
{
    public interface ICacheAdmin
    {
        Task ClearCache();
        Task<bool> ExistsKeyInCache(string key);
        List<string> GetKeys(string partialKey);
    }
}
