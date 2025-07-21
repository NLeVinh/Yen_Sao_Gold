using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs.Category;
using gold_server.DTOs.Image;
using gold_server.Exceptions;
using gold_server.Models;
using Microsoft.EntityFrameworkCore;

namespace gold_server.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly GoldServerContext _context;
        private readonly IImageService _imageService;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(GoldServerContext context, IImageService imageService, ILogger<CategoryService> logger)
        {
            _context = context;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _context.CATEGORIEs
                .Select(c => new CategoryDto
                {
                    ID_Category = c.ID_Category,
                    Name = c.Name,
                    Images = c.CATEGORY_IMAGEs.Select(ci => new ImageDto
                    {
                        ID_Image = ci.ID_Image,
                        URL = ci.ImageNavigation.URL
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _context.CATEGORIEs
                .Include(c => c.CATEGORY_IMAGEs)
                .ThenInclude(ci => ci.ImageNavigation)
                .FirstOrDefaultAsync(c => c.ID_Category == id);

            if (category == null)
                throw new AppException("Category not found");

            return new CategoryDto
            {
                ID_Category = category.ID_Category,
                Name = category.Name,
                Images = category.CATEGORY_IMAGEs.Select(ci => new ImageDto
                {
                    ID_Image = ci.ID_Image,
                    URL = ci.ImageNavigation.URL
                }).ToList()
            };
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, int userId)
        {
            var entity = new CATEGORY
            {
                Name = dto.Name,
                CreateDate = DateTime.UtcNow,
                CreateBy = userId
            };

            _context.CATEGORIEs.Add(entity);
            await _context.SaveChangesAsync();

            // Handle images
            if (dto.NewImages != null && dto.NewImages.Any())
            {
                string folder = $"categories/{entity.ID_Category}";

                int sortOrder = 1;
                foreach (var img in dto.NewImages)
                {
                    var uploaded = await _imageService.UploadImageAsync(img, folder);
                    _context.CATEGORY_IMAGEs.Add(new CATEGORY_IMAGE
                    {
                        ID_Category = entity.ID_Category,
                        ID_Image = uploaded.ID_Image,
                        SortOrder = sortOrder++
                    });
                }
                await _context.SaveChangesAsync();
            }

            return await GetByIdAsync(entity.ID_Category);
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto dto, int userId)
        {
            var category = await _context.CATEGORIEs.FindAsync(id);
            if (category == null)
                throw new AppException("Category not found");

            category.Name = dto.Name ?? category.Name;
            category.UpdateBy = userId;
            category.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await UpdateCategoryImagesAsync(id, dto);
        }

        private async Task UpdateCategoryImagesAsync(int categoryId, UpdateCategoryDto dto)
        {
            if (dto.ImagesToKeep != null && dto.ImagesToKeep.Any())
            {
                await RemoveUnwantedCategoryImagesAsync(categoryId, dto.ImagesToKeep);
            }
            else
            {
                await RemoveAllCategoryImagesAsync(categoryId);
            }

            if (dto.NewImages != null && dto.NewImages.Any())
            {
                await AddNewCategoryImagesAsync(categoryId, dto.NewImages);
            }

            var finalIds = await GetAllLinkedImageIdsForCategory(categoryId);
            await _imageService.CleanUpCloudinaryFolderAsync($"categories/{categoryId}", finalIds);
        }

        private async Task RemoveUnwantedCategoryImagesAsync(int categoryId, List<int> imagesToKeep)
        {
            var toRemoveLinks = _context.CATEGORY_IMAGEs
                .Where(x => x.ID_Category == categoryId && !imagesToKeep.Contains(x.ID_Image));

            var toRemoveIds = toRemoveLinks.Select(x => x.ID_Image).ToList();

            _context.CATEGORY_IMAGEs.RemoveRange(toRemoveLinks);
            await _context.SaveChangesAsync();

            await _imageService.DeleteImagesByIdsAsync(toRemoveIds);
        }

        private async Task RemoveAllCategoryImagesAsync(int categoryId)
        {
            var allLinks = _context.CATEGORY_IMAGEs.Where(x => x.ID_Category == categoryId);
            var allIds = allLinks.Select(x => x.ID_Image).ToList();

            _context.CATEGORY_IMAGEs.RemoveRange(allLinks);
            await _context.SaveChangesAsync();

            await _imageService.DeleteImagesByIdsAsync(allIds);
        }

        private async Task AddNewCategoryImagesAsync(int categoryId, List<IFormFile> newImages)
        {
            string folder = $"categories/{categoryId}";
            int sortOrder = 1;

            foreach (var img in newImages)
            {
                var newUploaded = await _imageService.UploadImageAsync(img, folder);
                _context.CATEGORY_IMAGEs.Add(new CATEGORY_IMAGE
                {
                    ID_Category = categoryId,
                    ID_Image = newUploaded.ID_Image,
                    SortOrder = sortOrder++
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task<List<int>> GetAllLinkedImageIdsForCategory(int categoryId)
        {
            return await _context.CATEGORY_IMAGEs
                .Where(x => x.ID_Category == categoryId)
                .Select(x => x.ID_Image)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.CATEGORIEs.FindAsync(id);
            if (category == null)
                throw new AppException("Category not found");

            await RemoveAllCategoryImagesAsync(id);
            _context.CATEGORIEs.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}