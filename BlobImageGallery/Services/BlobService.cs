using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlobImageGallery.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        public async Task<List<string>> GetBlobUrlsAsync()
        {
            var urls = new List<string>();
            await foreach (var blobItem in _containerClient.GetBlobsAsync())
            {
                var uri = _containerClient.GetBlobClient(blobItem.Name).Uri.ToString();
                urls.Add(uri);
            }
            return urls;
        }
    }
}
