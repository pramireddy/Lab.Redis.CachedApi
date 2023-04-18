using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.Azure.StorageService
{
    public interface IStorageService
    {
        Task<BlobContentInfo> UpaloadUserLogsAsync(string blobContainerName, string blobName, User user);
        Task<string> RetrieveBlobDataAsync(string blobContainerName, string blobName);
    }
}
