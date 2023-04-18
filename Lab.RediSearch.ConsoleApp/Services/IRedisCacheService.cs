using NRedisStack.Search;

namespace Lab.RediSearch.ConsoleApp.Services
{
    public interface IRedisCacheService
    {
        Task<bool> CreateIndexAsync(string indexName, FTCreateParams ftCreateParams, Schema schema);
        Task<bool> DropIndexAsync(string indexName);

        Task InsertJsonDocumentsAsync<T>(IEnumerable<T> items, string keyPrefix, string keyFieldName);

        Task<IEnumerable<T?>?> SearchAsync<T>(string searchIndex, Query query);

        Task<T?> GetJsonDocument<T>(string key);
    }
}
