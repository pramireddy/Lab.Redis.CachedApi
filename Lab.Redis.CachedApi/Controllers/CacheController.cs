using AutoFixture;
using Lab.Redis.CachedApi.Cache;
using Lab.Redis.CachedApi.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab.Redis.CachedApi.Controllers
{
    [ApiController]
    [Route("[controller]/Users")]
    public class CacheController : ControllerBase
    {
        private readonly ILogger<CacheController> logger;
        private readonly IRedisCacheService redisCacheService;
        private readonly List<User> users = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="connectionMultiplexer"></param>
        public CacheController(ILogger<CacheController> logger,
                               IConnectionMultiplexer connectionMultiplexer,
                               IRedisCacheService redisCacheService)
        {
            this.logger = logger;
            this.redisCacheService = redisCacheService;
            this.users = GetUserData();
        }


        [HttpPost]
        public async Task Post()
        {
            await redisCacheService.SetUserDataAsync(users);
        }

        [HttpDelete]
        public async Task Delete()
        {
            await redisCacheService.RemoveUserDataAsync(users);
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {

            return users.OrderBy( x=> x.PersonNumber);
        }

        [HttpGet("Search/{key}")]
        public async Task<IEnumerable<User>> Search(string key)
        {
            var result = await redisCacheService.SearchUsers(key);

            return result.OrderBy( x => x.PersonNumber);
        }

        [HttpGet("{key}")]
        public async Task<User> Get(string key)
        {
            var result = await redisCacheService.GetUserAsync(key);

            return result;
        }

        private List<User> GetUserData()
        {
            if (users.Any())
            {
                return users;
            }
            //var users = new List<User>();

            for (int x = 100; x < 125; x++)
            {
                var user = new User
                {
                    PersonNumber = x,
                    FirstName = GetFirstName(x),
                    LastName = $"LastName{x}",
                    Email = $"{GetEmail(x)}@test.com",

                };

                users.Add(user);

            }

            return users;
        }

        private static string GetFirstName(int number)
        {
            if (number % 3 == 0 && number % 5 == 0)
            {
                return "FizzBuzz";
            }
            else if (number % 5 == 0)
            {
                return "Buzz";
            }
            else if (number % 3 == 0)
            {
                return "Fizz";
            }
            else
            {
                return $"FirstName{number}";
            }
        }

        private static string GetEmail(int number)
        {
            if (number % 3 == 0 && number % 5 == 0)
            {
                return "prasad";
            }
            else if (number % 5 == 0)
            {
                return "akhi";
            }
            else if (number % 3 == 0)
            {
                return "veer";
            }
            else
            {
                return $"email{number}";
            }
        }
    }

}