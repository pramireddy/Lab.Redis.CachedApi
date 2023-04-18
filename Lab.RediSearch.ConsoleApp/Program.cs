using System.Text.Json;
using Lab.RediSearch.ConsoleApp.Services;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;
using StackExchange.Redis;

namespace Lab.RediSearch.ConsoleApp
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var users = GetUserData();

            var UserKeyPrefix = "user:";
            var searchIndexJSON = "JSON_testIndex";


            var pwd = "X2sNOeh0vFZAU5ZLhOLjI9lXKJQ7MVOh";
            IConnectionMultiplexer connectionMultiplexer =
                ConnectionMultiplexer.Connect($"redis-11078.c256.us-east-1-2.ec2.cloud.redislabs.com:11078,password={pwd}");


            var redisCacheService = new RedisCacheService(connectionMultiplexer);

            try
            {

                // drop and create index

                if (await redisCacheService.DropIndexAsync(searchIndexJSON))
                {
                    var schema = new Schema()
                        .AddTextField(name: new FieldName(name: "$.FirstName", "FirstName"), withSuffixTrie: true)
                        .AddTextField(name: new FieldName(name: "$.LastName", "LastName"), withSuffixTrie: true)
                        .AddTextField(name: new FieldName(name: "$.Email", "Email"), withSuffixTrie: true);

                    var ftCreateParams = new FTCreateParams()
                        .On(IndexDataType.JSON)
                        .Prefix(UserKeyPrefix);

                    await redisCacheService.CreateIndexAsync(searchIndexJSON, ftCreateParams, schema);
                }


                //// load users
                //await redisCacheService.InsertJsonDocumentsAsync(users, UserKeyPrefix, "PersonNumber");

                //// search users

                //var query = new Query("*buzz*").Limit(0, 50);
                //var query = new Query("'@FirstName:(buzz)'").Limit(0, 50);
                var query = new Query("'@Email:(*pra*)'").Limit(0, 50);
                var searchResults = await redisCacheService.SearchAsync<User>(searchIndexJSON, query);

                foreach (var item in searchResults)
                {
                    Console.WriteLine($"FirstName: {item.FirstName}, LastName: {item.LastName},Email: {item.Email}");
                }

                Console.WriteLine("\n");
                //IDatabase db = connectionMultiplexer.GetDatabase();

                //IJsonCommands json = db.JSON();

                foreach (var user in users)
                {
                    //var delResult = json.Del($"{UserKeyPrefix}{user.PersonNumber}", "$");

                    //Console.WriteLine(delResult);

                    //var options = new JsonSerializerOptions
                    //{
                    //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    //};



                    //var jsonString = JsonSerializer.Serialize(user, options);
                    //var renVal = json.Set($"{UserKeyPrefix}{user.PersonNumber}", "$", jsonString, When.Always);

                    //var renVal = json.Set($"{UserKeyPrefix}{user.PersonNumber}", "$", user, When.Always);

                    var result = await redisCacheService.GetJsonDocument<User>($"{UserKeyPrefix}{user.PersonNumber}");
                    if (result != null && result.Email.Contains("prasad"))
                    {
                        Console.WriteLine($"FirstName: {result.FirstName}, LastName: {result.LastName},Email: {result.Email}");
                    }

                    //if (result != null && result.FirstName.Contains("Buzz"))
                    //{
                    //    Console.WriteLine($"FirstName: {result.FirstName}, LastName: {result.LastName},Email: {result.Email}");
                    //}

                    //Console.WriteLine($"FirstName: {user.FirstName}, LastName: {user.LastName},Email: {user.Email}");
                    //db.HashSet($"{HASHUserKeyPrefix}{user.PersonNumber}", new HashEntry[] 
                    //{
                    //    new("PersonNumber", user.PersonNumber),
                    //    new("FirstName", user.FirstName),
                    //    new("LastName", user.LastName),
                    //    new ("Email",user.Email)
                    //});
                }
            }
            catch (Exception ex)
            {
                var log = ex.Message;
            }

            Console.WriteLine("Hello, World!");
        }




        private static List<User> GetUserData()
        {
            List<User> users = new();

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


    //public class Program
    //{

    //    public static async Task Main(string[] args)
    //    {
    //       var users = GetUserData();

    //       var UserKeyPrefix = "user:";
    //       var HASHUserKeyPrefix = "hashuser:";

    //        var searchIndex = "testIndex";
    //        var searchIndexJSON = "JSON_testIndex";
    //        var searchIndexHASH = "HASH_TestIndex";

    //        string pwd = "X2sNOeh0vFZAU5ZLhOLjI9lXKJQ7MVOh";


    //        IConnectionMultiplexer connectionMultiplexer =
    //            ConnectionMultiplexer.Connect($"redis-11078.c256.us-east-1-2.ec2.cloud.redislabs.com:11078,password={pwd}");

    //        IDatabase db = connectionMultiplexer.GetDatabase();

    //        IJsonCommands json = db.JSON();

    //        ISearchCommands ft = db.FT();

    //        ISearchCommandsAsync ftAsync = db.FT();



    //        try
    //        {
    //            //NReJSONSerializer.SerializerProxy = new TestJsonSerializer();

    //            var schema = new Schema()
    //                .AddTextField(name: "$.firstName", withSuffixTrie: true)
    //                .AddTextField(name: "$.lastName", withSuffixTrie: true)
    //           .AddTextField(name: "$.email", withSuffixTrie: true);

    //            var schema1 = new Schema()
    //                .AddTagField(new FieldName("$.FirstName","FirstName"));
    //                //.AddTextField(name: "LastName")
    //                //.AddTextField(name: "Email");

    //            var params1 = new FTCreateParams()
    //                             .On(IndexDataType.JSON)
    //                             .Prefix(UserKeyPrefix);

    //            //ft.DropIndex(searchIndex);

    //            //ft.Create(searchIndex, params1, schema1);
    //            //ft.Create(searchIndex, params1, schema1);

    //            //ft.DropIndex(searchIndexJSON);

    //            //ft.Create(searchIndexJSON, params1, schema);

    //            //ft.Create("test", new FTCreateParams().On(IndexDataType.JSON).Prefix("doc:"),
    //            //    new Schema().AddTagField(new FieldName("$.name", "name")));

    //            //for (int i = 0; i < 10; i++)
    //            //{
    //            //    json.Set("doc:" + i, "$", "{\"name\":\"foo\"}");
    //            //}

    //            //var res = ft.Search("test", new Query("@name:{foo}"));
    //            //var docs = res.ToJson().ToList();

    //            //ft.DropIndex(searchIndexHASH);

    //            //ft.Create(searchIndexHASH, new FTCreateParams().On(IndexDataType.HASH).Prefix(HASHUserKeyPrefix), schema);

    //            //var searchText = "Fizz";

    //            //var queryString = $"@FirsName|LastName|Email:{searchText}";

    //            //var queryString = $"@Email:*{searchText}*";

    //            //var queryString = $"@FirstName:Fizz*";

    //            //var searchList = ft.Search(searchIndexHASH, new Query("veer"));

    //            //var searchListJSON = ft.Search(searchIndexHASH, new Query("@FirstName:{Fizz}"));

    //            //var result = json.Get<User>($"{UserKeyPrefix}124");

    //            var queryString = "@email:veer*";

    //            var searchR = ft.Search(searchIndexJSON, new Query(queryString));

    //            var getALl1 = await ftAsync.SearchAsync(searchIndexJSON, new Query(queryString));

    //            queryString = "*veer*";

    //            var getALl = await ftAsync.SearchAsync(searchIndexJSON, new Query(queryString).Limit(0,50));

    //            var docResult = getALl.ToJson();
    //            //var docResult = (getALl.ToJson() ?? Array.Empty<string>()).ToArray();

    //            //var objectResult = JsonSerializer.Deserialize<User>(docResult.FirstOrDefault());

    //            var searchResult = docResult.Select(jsonString => JsonSerializer.Deserialize<User1>(jsonString)).ToList();


    //            foreach (var user in users)
    //            {
    //                //var delResult = json.Del($"{UserKeyPrefix}{user.PersonNumber}", "$");

    //                //Console.WriteLine(delResult);

    //                //var options = new JsonSerializerOptions
    //                //{
    //                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //                //};



    //                //var jsonString = JsonSerializer.Serialize(user, options);
    //                //var renVal = json.Set($"{UserKeyPrefix}{user.PersonNumber}", "$", jsonString, When.Always);

    //                //var renVal = json.Set($"{UserKeyPrefix}{user.PersonNumber}", "$", user, When.Always);

    //                //var result = json.Get<User1>($"{UserKeyPrefix}{user.PersonNumber}");
    //                //if (result != null && result.email.Contains("veer"))
    //                //{
    //                //    Console.WriteLine($"FirstName: {result.firstName}, LastName: {result.lastName},Email: {result.email}");
    //                //}

    //                //if (result != null && result.FirstName.Contains("Fizz"))
    //                //{
    //                //    Console.WriteLine($"FirstName: {result.FirstName}, LastName: {result.LastName},Email: {result.Email}");
    //                //}

    //                //Console.WriteLine($"FirstName: {user.FirstName}, LastName: {user.LastName},Email: {user.Email}");
    //                //db.HashSet($"{HASHUserKeyPrefix}{user.PersonNumber}", new HashEntry[] 
    //                //{
    //                //    new("PersonNumber", user.PersonNumber),
    //                //    new("FirstName", user.FirstName),
    //                //    new("LastName", user.LastName),
    //                //    new ("Email",user.Email)
    //                //});
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            var log = ex.Message;
    //        }

    //        Console.WriteLine("Hello, World!");
    //    }




    //    private static List<User> GetUserData()
    //    {
    //        List<User> users = new();

    //        for (int x = 100; x < 125; x++)
    //        {
    //            var user = new User
    //            {
    //                PersonNumber = x,
    //                FirstName = GetFirstName(x),
    //                LastName = $"LastName{x}",
    //                Email = $"{GetEmail(x)}@test.com",

    //            };

    //            users.Add(user);

    //        }

    //        return users;
    //    }

    //    private static string GetFirstName(int number)
    //    {
    //        if (number % 3 == 0 && number % 5 == 0)
    //        {
    //            return "FizzBuzz";
    //        }
    //        else if (number % 5 == 0)
    //        {
    //            return "Buzz";
    //        }
    //        else if (number % 3 == 0)
    //        {
    //            return "Fizz";
    //        }
    //        else
    //        {
    //            return $"FirstName{number}";
    //        }
    //    }

    //    private static string GetEmail(int number)
    //    {
    //        if (number % 3 == 0 && number % 5 == 0)
    //        {
    //            return "prasad";
    //        }
    //        else if (number % 5 == 0)
    //        {
    //            return "akhi";
    //        }
    //        else if (number % 3 == 0)
    //        {
    //            return "veer";
    //        }
    //        else
    //        {
    //            return $"email{number}";
    //        }
    //    }
    //}
}