using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.CloudStorage
{
    internal class GoogleCloudStorageService : ICloudStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorageService(IConfiguration configuration)
        {
            var googleCredential = GoogleCredential.FromFile(configuration["GCS:GoogleCredentialFile"]);
            _storageClient = StorageClient.Create(googleCredential);
            _bucketName = configuration["GCS:GoogleCloudStorageBucket"];
        }

        public async Task<string> UploadAsync(IFormFile file, string filename)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var dataObject = await _storageClient.UploadObjectAsync(_bucketName, filename, null, memoryStream);
            return dataObject.MediaLink;
        }

        public Task DeleteAsync(string filename)
        {
            return _storageClient.DeleteObjectAsync(_bucketName, filename);
        }
    }
}
