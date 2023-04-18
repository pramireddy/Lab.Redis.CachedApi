using AutoFixture;
using Lab.Redis.CachedApi.Models;
using Microsoft.AspNetCore.Routing.Template;

namespace Lab.Redis.CachedApi.Cache
{
    public interface IRedisCacheService
    {
        Task SetUserDataAsync(IEnumerable<User> users);
        Task RemoveUserDataAsync(IEnumerable<User> users);
        Task<User> GetUserAsync(string key);
        Task<IEnumerable<User>> SearchUsers(string query);
    }
}
