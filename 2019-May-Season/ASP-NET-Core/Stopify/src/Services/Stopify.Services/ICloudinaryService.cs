using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName);
    }
}
