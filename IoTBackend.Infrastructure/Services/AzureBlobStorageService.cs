using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace IoTBackend.Infrastructure.Services
{
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient, string containerName)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = containerName;
        }

        public async Task<IEnumerable<string>> ListFolderAsync(string path, bool foldersOnly = false)
        {
            var pathWithTrailingSlash = path.EndsWith("/") ? path : path + "/";
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var results = new List<string>();
            var blobItems = containerClient.GetBlobsByHierarchyAsync(prefix: pathWithTrailingSlash, delimiter:"/");
            await foreach (var blob in blobItems)
            {
                if ((!foldersOnly || blob.IsBlob) && foldersOnly) continue;
                // Clear path from the names
                var name = (blob.Blob?.Name ?? blob.Prefix).TrimEnd('/');
                if (name.StartsWith(pathWithTrailingSlash))
                {
                    name = name[pathWithTrailingSlash.Length..];
                }
                results.Add(name);
            }
            return results;
        }

        public async Task<Stream> ReadDocumentStreamAsync(string path)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blockBlob = containerClient.GetBlobClient(path);
            return await blockBlob.OpenReadAsync();
        }
    }
}