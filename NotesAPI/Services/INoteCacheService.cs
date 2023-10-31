namespace NotesAPI.Services
{
    public interface INoteCacheService: INoteService
    {
        void ClearCache();
        bool ExistsKeyInCache(string key);
    }
}
