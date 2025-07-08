using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gold_server.DTOs;
using gold_server.DTOs.common;
using gold_server.DTOs.Product;

namespace gold_server.Services
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductDto>> GetAllAsync(string search, int? categoryId, int? statusId, int page, int pageSize);
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto dto, int createByUserId);
        Task<bool> UpdateAsync(int id, UpdateProductDto dto, int updateByUserId);
        Task<bool> DeleteAsync(int id);
    }
}