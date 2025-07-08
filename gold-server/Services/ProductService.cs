using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs.common;
using gold_server.DTOs.Product;
using gold_server.Exceptions;
using gold_server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace gold_server.Services
{
    public class ProductService : IProductService
    {
        private readonly GoldServerContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(GoldServerContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResultDto<ProductDto>> GetAllAsync(string search, int? categoryId, int? statusId, int page, int pageSize)
        {
            var query = _context.PRODUCTs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
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
                    ID_Status = p.ID_Status
                })
                .ToListAsync();

            return new PagedResultDto<ProductDto>(items, totalCount, page, pageSize);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            try
            {
                var product = await _context.PRODUCTs.FindAsync(id);
                if (product == null)
                    throw new AppException("Product not found");

                return new ProductDto
                {
                    ID_Product = product.ID_Product,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ID_Category = product.ID_Category,
                    InStock = product.InStock,
                    SoldQuantity = product.SoldQuantity,
                    ID_Status = product.ID_Status
                };
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

                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.ID_Category = dto.ID_Category;
                product.InStock = dto.InStock;
                product.SoldQuantity = dto.SoldQuantity;
                product.ID_Status = dto.ID_Status;
                product.UpdateBy = updateByUserId;
                product.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
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

                _context.PRODUCTs.Remove(product);
                await _context.SaveChangesAsync();
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
    }
}