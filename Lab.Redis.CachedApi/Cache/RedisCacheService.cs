using Lab.Redis.CachedApi.Models;
using NReJSON;
using RediSearchClient;
using RediSearchClient.Indexes;
using RediSearchClient.Query;
using StackExchange.Redis;
using System.Text.Json;

namespace Lab.Redis.CachedApi.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase redisdatabase;
        private readonly string UserKeyPrefix = "user:";
        private readonly string USER_INDEX = "user_index";

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            this.redisdatabase = connectionMultiplexer.GetDatabase();
        }
        public async Task<User> GetUserAsync(string key)
        {
            RedisResult result = await redisdatabase.JsonGetAsync($"{UserKeyPrefix}{key}");
            return  result.IsNull ? default! : JsonSerializer.Deserialize<User>(result.ToString()!)!;
        }

        public async Task SetUserDataAsync(IEnumerable<User> users)
        {
            try
            {
                NReJSONSerializer.SerializerProxy = new TestJsonSerializer();

                foreach (var user in users)
                {
                    await redisdatabase.JsonSetAsync($"{UserKeyPrefix}{user.PersonNumber}", user, commandFlags: CommandFlags.FireAndForget);
                }
            }
            catch (Exception ex)
            {
                var log = ex.Message;
            }

        }


        public async Task<IEnumerable<User>> SearchUsers(string query)
        {

            try
            {
                await redisdatabase.ExecuteAsync("FT.DROPINDEX", USER_INDEX);
            }
            catch (Exception ex)
            {
                var log = ex.Message;
            }

            try
            {
                var indexDefintion = RediSearchIndex
                      .OnJson()
                      .ForKeysWithPrefix("user:")
                      .WithSchema(
                          x => x.Numeric("$.PersonNumber", "PersonNumber"),
                          x => x.Text("$.FirstName", "FirstName"),
                          x => x.Text("$.LastName", "LastName")
                      )
                      .Build();


                await redisdatabase.CreateIndexAsync(USER_INDEX, indexDefintion);

                var searchQuery = RediSearchQuery
                    .On(USER_INDEX)
                    .UsingQuery($"@FirstName:{query}*")
                    .Build();

                var result = await redisdatabase.SearchAsync(searchQuery);

                var users = result.Select(x =>
                {

                    return JsonSerializer.Deserialize<User>(x.Fields.FirstOrDefault().Value.ToString()!);

                });

                return users!;
            }
            catch (Exception ex)
            {
                var log = ex.Message;
            }

            return new List<User>();
        }

    }
}
