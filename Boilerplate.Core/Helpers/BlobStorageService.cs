using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Boilerplate.Core.Helpers
{
    public class BlobStorageService
    {
        private const string ContainerName = "Boilerplate";
        private readonly BlobServiceClient _mBlobServiceClient;
        private BlobContainerClient _mBlobContainerClient;

        public BlobStorageService()
        {
            _mBlobServiceClient =
                new BlobServiceClient(
                    "DefaultEndpointsProtocol=https;AccountName=cmos;AccountKey=LYm0KgHr9PextTTteqZKf4DcV9rNbps/XowJ2HjPtrqnngKEU0YjfloqVQDwJ25fvPAN1cllkAADdyo0p2Wlxw==;EndpointSuffix=core.windows.net");
        }

        public async Task<Stream> DownloadAsync(string fileName, string containerName = ContainerName)
        {
            _mBlobContainerClient = _mBlobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = _mBlobContainerClient.GetBlobClient(fileName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task<string> UploadAsync(IFormFile file, string name, string containerName = ContainerName)
        {
            _mBlobContainerClient = _mBlobServiceClient.GetBlobContainerClient(containerName);
            await _mBlobContainerClient.CreateIfNotExistsAsync();
            await _mBlobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            var blobClient = _mBlobContainerClient.GetBlobClient($@"{Guid.NewGuid()}_{name}");
            var blobOptions = new BlobUploadOptions
                {HttpHeaders = new BlobHttpHeaders {ContentType = file.ContentType}};
            await blobClient.UploadAsync(file.OpenReadStream(), blobOptions);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}