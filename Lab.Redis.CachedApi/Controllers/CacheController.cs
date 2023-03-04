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
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ILogger<CacheController> logger;
        private readonly IRedisCacheService redisCacheService;

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

        }


        [HttpPost("Users")]
        public async Task Post()
        {
            List<User> users = GetUserData();

            //var fixtute = new Fixture();
            //fixtute.RepeatCount = 10;
            //var fakeUsersList = fixtute.Create<IEnumerable<User>>();

            await redisCacheService.SetUserDataAsync(users);
        }


        [HttpGet("Users")]
        public IEnumerable<User> Get()
        {
            List<User> users = GetUserData();

            //var fixtute = new Fixture();
            //fixtute.RepeatCount = 10;
            //var fakeUsersList = fixtute.Create<IEnumerable<User>>();

            return users.OrderBy( x=> x.PersonNumber);
        }

        [HttpGet("Users/Search/{key}")]
        public async Task<IEnumerable<User>> Search(string key)
        {
            var result = await redisCacheService.SearchUsers(key);

            return result.OrderBy( x => x.PersonNumber);
        }

        [HttpGet("Users/{key}")]
        public async Task<User> Get(string key)
        {
            var result = await redisCacheService.GetUserAsync(key);

            return result;
        }

        private static List<User> GetUserData()
        {
            var users = new List<User>();

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