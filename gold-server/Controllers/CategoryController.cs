using System.Security.Claims;
using gold_server.DTOs.Category;
using gold_server.DTOs.common;
using gold_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gold_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authenticated user by default for all actions
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(new ApiResponseDto<List<CategoryDto>>(result));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(new ApiResponseDto<CategoryDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new ApiResponseDto<string>("Unauthorized: No user ID found in token"));

            if (dto.NewImages != null && dto.NewImages.Count > 1)
                return BadRequest(new ApiResponseDto<string>("You can only upload up to 1 images for a category."));


            int userId = int.Parse(userIdClaim);

            var result = await _service.CreateAsync(dto, userId);
            return Ok(new ApiResponseDto<CategoryDto>(result, "Category created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCategoryDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new ApiResponseDto<string>("Unauthorized: No user ID found in token"));

            if (dto.NewImages != null && dto.NewImages.Count > 1)
                return BadRequest(new ApiResponseDto<string>("You can only upload up to 1 images for a category."));

            int userId = int.Parse(userIdClaim);

            await _service.UpdateAsync(id, dto, userId);
            return Ok(new ApiResponseDto<string>("Category updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new ApiResponseDto<string>("Category deleted successfully"));
        }
    }
}