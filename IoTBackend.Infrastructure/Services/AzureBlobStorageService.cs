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

        public async Task<IEnumerable<string>> ListFolderAsync(string path)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var results = new List<string>();
            await foreach (var blob in containerClient.GetBlobsAsync())
            {
                results.Add(blob.Name);
            }
            return results;
        }
        
        public async Task<string> ReadDocumentAsStringAsync(string path)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blockBlob = containerClient.GetBlobClient(path);
            var stream = new MemoryStream();
            await blockBlob.DownloadToAsync(stream);
            using var reader = new StreamReader(stream);
            var result = (await reader.ReadToEndAsync()).Trim();
            return result;
        }

        public async Task ReadDocumentStreamAsync(string path, Stream stream)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blockBlob = containerClient.GetBlobClient(path);
            await blockBlob.DownloadToAsync(stream);
        }
    }
}