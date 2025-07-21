using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using gold_server.Configs;
using gold_server.DTOs.Image;
using gold_server.Exceptions;
using gold_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace gold_server.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly GoldServerContext _context;
        private readonly ILogger<ImageService> _logger;

        public ImageService(
            IOptions<CloudinarySettings> config,
            GoldServerContext context,
            ILogger<ImageService> logger)
        {
            _context = context;
            _logger = logger;

            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        private string ExtractPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex == -1 || uploadIndex + 1 >= segments.Length)
                return null;

            var publicIdWithExt = string.Join("/", segments.Skip(uploadIndex + 1));
            var dotIndex = publicIdWithExt.LastIndexOf('.');
            return dotIndex > 0 ? publicIdWithExt.Substring(0, dotIndex) : publicIdWithExt;
        }

        public async Task DeleteImageFromCloudinaryAsync(string url)
        {
            try
            {
                var publicId = ExtractPublicIdFromUrl(url);
                if (string.IsNullOrEmpty(publicId))
                {
                    _logger.LogWarning("Could not extract PublicId from URL {Url}", url);
                    return;
                }

                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                if (result.Result != "ok")
                    _logger.LogWarning("Cloudinary deletion failed for {PublicId}", publicId);
                else
                    _logger.LogInformation("Deleted image {PublicId} from Cloudinary", publicId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image from Cloudinary: {Url}", url);
            }
        }

        public async Task<ImageDto> UploadImageAsync(IFormFile file, string folder)
        {
            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = false
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // throw new AppException("Failed to upload image to Cloudinary");
                    _logger.LogError("Cloudinary Upload Failed. Status: {Status}, Error: {Error}", result.StatusCode, result.Error?.Message);
                    throw new AppException("Failed to upload image to Cloudinary");
                }

                var image = new IMAGE
                {
                    URL = result.SecureUrl.AbsoluteUri
                };

                _context.IMAGEs.Add(image);
                await _context.SaveChangesAsync();

                return new ImageDto
                {
                    ID_Image = image.ID_Image,
                    URL = image.URL
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image to Cloudinary folder {Folder}", folder);
                throw new AppException("Error uploading image");
            }
        }

        public async Task<IEnumerable<ImageDto>> GetAllImagesAsync()
        {
            var images = await _context.IMAGEs
                .Select(i => new ImageDto
                {
                    ID_Image = i.ID_Image,
                    URL = i.URL
                })
                .ToListAsync();

            return images;
        }

        public async Task DeleteImageAsync(int id)
        {
            try
            {
                var img = await _context.IMAGEs.FindAsync(id);
                if (img == null)
                    throw new AppException("Image not found");

                _context.IMAGEs.Remove(img);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image {Id}", id);
                throw new AppException("Error deleting image");
            }
        }
        public async Task DeleteFolderFromCloudinaryAsync(string folder)
        {
            try
            {
                var deleteParams = new DelResParams
                {
                    Prefix = folder
                };

                var result = await _cloudinary.DeleteResourcesAsync(deleteParams);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    _logger.LogWarning("Failed to delete resources in folder {Folder}", folder);
                else
                    _logger.LogInformation("Deleted all resources in folder {Folder}", folder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting folder {Folder}", folder);
            }
        }
        public async Task DeleteImagesByIdsAsync(List<int> imageIds)
        {
            if (imageIds == null || !imageIds.Any())
                return;

            var images = await _context.IMAGEs
                .Where(i => imageIds.Contains(i.ID_Image))
                .ToListAsync();

            foreach (var img in images)
            {
                await DeleteImageFromCloudinaryAsync(img.URL);
                _context.IMAGEs.Remove(img);
            }

            await _context.SaveChangesAsync();
        }
        public async Task CleanUpCloudinaryFolderAsync(string folder, List<int> allowedImageIds)
        {
            try
            {
                // Tìm tất cả ảnh trong IMAGES có URL chứa folder
                var allImagesInFolder = await _context.IMAGEs
                    .Where(img => img.URL.Contains($"/{folder}/"))
                    .ToListAsync();

                // Lọc ra ảnh cần xóa
                var toDelete = allImagesInFolder
                    .Where(img => !allowedImageIds.Contains(img.ID_Image))
                    .ToList();

                if (!toDelete.Any())
                {
                    _logger.LogInformation("No unused images to clean in folder {Folder}", folder);
                    return;
                }

                // Xoá trên Cloudinary và DB
                foreach (var img in toDelete)
                {
                    await DeleteFolderFromCloudinaryAsync(img.URL);
                    _context.IMAGEs.Remove(img);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} unused images in folder {Folder}", toDelete.Count, folder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up Cloudinary folder {Folder}", folder);
                throw new AppException("Failed to clean up Cloudinary folder");
            }
        }
    }
}