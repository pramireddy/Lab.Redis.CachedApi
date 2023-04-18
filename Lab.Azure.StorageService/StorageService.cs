using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace Lab.Azure.StorageService
{
    public class StorageService : IStorageService
    {
        private BlobServiceClient _blobServiceClient;

        public StorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<BlobContentInfo> UpaloadUserLogsAsync(string blobContainerName, string blobName, User user)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            var fileName = $"{blobName}/{user.PersonNumber}.json";
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            var dataToUpload = JsonSerializer.Serialize(user);

            var dataBytes = Encoding.UTF8.GetBytes(dataToUpload);
            var contentStream = new MemoryStream(dataBytes);
            try
            {
                BlobContentInfo result = await blobClient.UploadAsync(contentStream);

                return result;
            }
            catch (Exception ex)
            {
                var logMessage = ex.Message;
            }
            
            return default;
        }

        public async Task<string> RetrieveBlobDataAsync(string blobContainerName, string blobName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            await using var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            var blobData = Encoding.UTF8.GetString(memoryStream.ToArray());
            return blobData;
        }

    }
}
