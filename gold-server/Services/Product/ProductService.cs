using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs.common;
using gold_server.DTOs.Product;
using gold_server.Exceptions;
using gold_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace gold_server.Services
{
    public class ProductService : IProductService
    {
        private readonly GoldServerContext _context;
        private readonly ILogger<ProductService> _logger;
        private readonly IImageService _imageService;

        public ProductService(GoldServerContext context, ILogger<ProductService> logger, IImageService imageService)
        {
            _context = context;
            _logger = logger;
            _imageService = imageService;
        }

        public async Task<PagedResultDto<ProductDto>> GetAllAsync(string? search, int? categoryId, int? statusId, int page, int pageSize)
        {
            var query = _context.PRODUCTs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(p => p.ID_Category == categoryId);

            if (statusId.HasValue)
                query = query.Where(p => p.ID_Status == statusId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.ID_Product)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    ID_Product = p.ID_Product,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ID_Category = p.ID_Category,
                    InStock = p.InStock,
                    SoldQuantity = p.SoldQuantity,
                    ID_Status = p.ID_Status,
                    Images = p.PRODUCT_IMAGEs.Select(pi => new DTOs.Image.ImageDto
                    {
                        ID_Image = pi.ID_Image,
                        URL = pi.ImageNavigation.URL
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResultDto<ProductDto>(items, totalCount, page, pageSize);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            try
            {
                var productWithImages = await _context.PRODUCTs
                    .Where(p => p.ID_Product == id)
                    .Select(p => new ProductDto
                    {
                        ID_Product = p.ID_Product,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ID_Category = p.ID_Category,
                        InStock = p.InStock,
                        SoldQuantity = p.SoldQuantity,
                        ID_Status = p.ID_Status,
                        Images = p.PRODUCT_IMAGEs.Select(pi => new DTOs.Image.ImageDto
                        {
                            ID_Image = pi.ID_Image,
                            URL = pi.ImageNavigation.URL
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (productWithImages == null)
                    throw new AppException("Product not found");

                return productWithImages;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by id {Id}", id);
                throw new AppException("Could not load product details");
            }
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto, int createByUserId)
        {
            try
            {
                var product = new PRODUCT
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    ID_Category = dto.ID_Category,
                    InStock = 0,
                    SoldQuantity = 0,
                    ID_Status = dto.ID_Status,
                    CreateDate = DateTime.UtcNow,
                    CreateBy = createByUserId
                };

                _context.PRODUCTs.Add(product);
                await _context.SaveChangesAsync();

                if (dto.Images != null && dto.Images.Any())
                {
                    string folder = $"products/{product.ID_Product}";
                    int sortOrder = 1;
                    foreach (var file in dto.Images)
                    {
                        var image = await _imageService.UploadImageAsync(file, folder);

                        var productImage = new PRODUCT_IMAGE
                        {
                            ID_Product = product.ID_Product,
                            ID_Image = image.ID_Image,
                            SortOrder = sortOrder++
                        };

                        _context.PRODUCT_IMAGEs.Add(productImage);
                    }
                    await _context.SaveChangesAsync();
                }

                return await GetByIdAsync(product.ID_Product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                throw new AppException("Failed to create product");
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto, int updateByUserId)
        {
            try
            {
                var product = await _context.PRODUCTs.FindAsync(id);
                if (product == null)
                    throw new AppException("Product not found");

                product.Name = dto.Name ?? product.Name;
                product.Description = dto.Description ?? product.Description;
                product.Price = dto.Price ?? product.Price;
                product.ID_Category = dto.ID_Category ?? product.ID_Category;
                product.InStock = dto.InStock ?? product.InStock;
                product.SoldQuantity = dto.SoldQuantity ?? product.SoldQuantity;
                product.ID_Status = dto.ID_Status ?? product.ID_Status;
                product.UpdateBy = updateByUserId;
                product.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await UpdateProductImagesAsync(id, dto);

                return true;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {Id}", id);
                throw new AppException("Failed to update product");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var product = await _context.PRODUCTs.FindAsync(id);
                if (product == null)
                    throw new AppException("Product not found");

                var images = _context.PRODUCT_IMAGEs.Where(x => x.ID_Product == id).ToList();

                foreach (var link in images)
                {
                    var img = await _context.IMAGEs.FindAsync(link.ID_Image);
                    if (img != null)
                    {
                        // Clean up Cloudinary for this image
                        await _imageService.DeleteFolderFromCloudinaryAsync(img.URL);

                        _context.IMAGEs.Remove(img);
                    }
                }

                _context.PRODUCT_IMAGEs.RemoveRange(images);
                await _context.SaveChangesAsync();

                _context.PRODUCTs.Remove(product);
                await _context.SaveChangesAsync();

                // Optional: Clean up entire folder on Cloudinary
                string folder = $"products/{id}";
                await _imageService.DeleteFolderFromCloudinaryAsync(folder);

                return true;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {Id}", id);
                throw new AppException("Failed to delete product");
            }
        }

        private async Task UpdateProductImagesAsync(int productId, UpdateProductDto dto)
        {
            try
            {
                if (dto.ImagesToKeep != null && dto.ImagesToKeep.Any())
                {
                    await RemoveUnwantedImageLinksAsync(productId, dto.ImagesToKeep);
                }
                else
                {
                    await RemoveAllImageLinksAsync(productId);
                }

                if (dto.NewImages != null && dto.NewImages.Any())
                {
                    await AddNewProductImagesAsync(productId, dto.NewImages);
                }

                var finalLinkedImageIds = await GetAllLinkedImageIdsForProduct(productId);

                await _imageService.CleanUpCloudinaryFolderAsync(
                    $"products/{productId}",
                    finalLinkedImageIds
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product images for product {ProductId}", productId);
                throw new AppException("Failed to update product images");
            }
        }
        private async Task<List<int>> GetAllLinkedImageIdsForProduct(int productId)
        {
            return await _context.PRODUCT_IMAGEs
                .Where(x => x.ID_Product == productId)
                .Select(x => x.ID_Image)
                .ToListAsync();
        }

        private async Task RemoveUnwantedImageLinksAsync(int productId, List<int> imagesToKeep)
        {
            var toRemoveLinks = _context.PRODUCT_IMAGEs
                .Where(x => x.ID_Product == productId && !imagesToKeep.Contains(x.ID_Image));

            var toRemoveIds = toRemoveLinks.Select(x => x.ID_Image).ToList();

            _context.PRODUCT_IMAGEs.RemoveRange(toRemoveLinks);
            await _context.SaveChangesAsync();

            await _imageService.DeleteImagesByIdsAsync(toRemoveIds);
        }

        private async Task RemoveAllImageLinksAsync(int productId)
        {
            var allLinks = _context.PRODUCT_IMAGEs.Where(x => x.ID_Product == productId);
            var allIds = allLinks.Select(x => x.ID_Image).ToList();

            _context.PRODUCT_IMAGEs.RemoveRange(allLinks);
            await _context.SaveChangesAsync();

            await _imageService.DeleteImagesByIdsAsync(allIds);
        }

        private async Task AddNewProductImagesAsync(int productId, List<IFormFile> newImages)
        {
            string folder = $"products/{productId}";
            int sortOrder = 1;

            foreach (var img in newImages)
            {
                var newUploaded = await _imageService.UploadImageAsync(img, folder);
                _context.PRODUCT_IMAGEs.Add(new PRODUCT_IMAGE
                {
                    ID_Product = productId,
                    ID_Image = newUploaded.ID_Image,
                    SortOrder = sortOrder++
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}