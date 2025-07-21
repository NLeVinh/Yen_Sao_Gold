using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using gold_server.DTOs.Image;

namespace gold_server.Services
{
    public interface IImageService
    {
        Task<ImageDto> UploadImageAsync(IFormFile file, string folder);
        Task<IEnumerable<ImageDto>> GetAllImagesAsync();
        Task DeleteImageAsync(int id);
        Task DeleteFolderFromCloudinaryAsync(string folder);
        Task DeleteImagesByIdsAsync(List<int> imageIds);
        Task CleanUpCloudinaryFolderAsync(string folder, List<int> allowedImageIds);
        Task DeleteImageFromCloudinaryAsync(string url);
    }
}