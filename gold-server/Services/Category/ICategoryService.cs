using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs.Category;

namespace gold_server.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto, int userId);
        Task UpdateAsync(int id, UpdateCategoryDto dto, int userId);
        Task DeleteAsync(int id);
    }
}