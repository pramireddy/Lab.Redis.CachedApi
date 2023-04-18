using System.Text.Json;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;

namespace Lab.RediSearch.ConsoleApp.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly  IJsonCommandsAsync _jsonCommands;
    private readonly ISearchCommandsAsync _searchCommands;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        var db = connectionMultiplexer.GetDatabase();
        _jsonCommands = db.JSON();
        _searchCommands = db.FT();
    }

    public async Task<bool> CreateIndexAsync(string indexName, FTCreateParams ftCreateParams, Schema schema)
    {
        return await _searchCommands.CreateAsync(indexName, ftCreateParams, schema);
    }

    public async Task<bool> DropIndexAsync(string indexName)
    {
        try
        {
            return await _searchCommands.DropIndexAsync(indexName);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task InsertJsonDocumentsAsync<T> (IEnumerable<T> items, string keyPrefix, string keyFieldName)
    {
        try
        {
            foreach (var item in items)
            {
                var keyFiledValue = GetKeyFiledValue(keyFieldName, item);

                await _jsonCommands.SetAsync($"{keyPrefix}{keyFiledValue}", "$", item);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }



    }

    private static int? GetKeyFiledValue<T>(string keyFieldName, T item)
    {
        var objectType = item?.GetType();
        var propertyInfo = objectType?.GetProperty(keyFieldName);
        return (int?)propertyInfo?.GetValue(item);
    }

    public async Task<IEnumerable<T?>?> SearchAsync<T>(string searchIndex, Query query)
    {
        var searchResult = await _searchCommands.SearchAsync(searchIndex, query);
        var result = searchResult.ToJson()?.Select(jsonString => JsonSerializer.Deserialize<T>(jsonString));
        return result;
    }

    public async Task<T?> GetJsonDocument<T>(string key)
    {
        return await _jsonCommands.GetAsync<T>(key);
    }
}