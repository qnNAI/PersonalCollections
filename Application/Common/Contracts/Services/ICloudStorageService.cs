using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Contracts.Services
{
    public interface ICloudStorageService
    {
        Task<string> UploadAsync(IFormFile file, string filename);
        Task DeleteAsync(string filename);
    }
}
