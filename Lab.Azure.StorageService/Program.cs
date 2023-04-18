using Azure.Storage.Blobs;
using System.Text.Json;

namespace Lab.Azure.StorageService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
            var storageService = new StorageService(blobServiceClient);

            //var user = new User
            //{
            //    PersonNumber = 102,
            //    FirstName = "FirstName Test",
            //    LastName = "LastName Test",
            //    Email = "email@test.com"
            //};

            ////var dataToLoad = JsonSerializer.Serialize(user);

            //var result = await storageService.UpaloadUserLogsAsync("log-storage", "test123", user);

            //user = new User
            //{
            //    PersonNumber = 103,
            //    FirstName = "FirstName1 Test",
            //    LastName = "LastName1 Test",
            //    Email = "email1@test.com"
            //};

            //result = await storageService.UpaloadUserLogsAsync("log-storage", "test123", user);

            var filename = $"test123/102.json";
            var result = await storageService.RetrieveBlobDataAsync("log-storage", filename);

            Console.WriteLine(result);

            var user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                
            });

            Console.WriteLine(user.PersonNumber);
;
            Console.WriteLine("Hello, World!");
        }
    }
}