using Microsoft.AspNetCore.Mvc;
using gold_server.DTOs;
using gold_server.DTOs.common;
using gold_server.Services;
using System.Threading.Tasks;
using gold_server.DTOs.Product;

namespace gold_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? categoryId, [FromQuery] int? statusId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllAsync(search, categoryId, statusId, page, pageSize);
            return Ok(new ApiResponseDto<PagedResultDto<ProductDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return Ok(new ApiResponseDto<ProductDto>(product));
        }

        [HttpPost]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            int userId = 1; // replace with real user from token
            var product = await _service.CreateAsync(dto, userId);
            return Ok(new ApiResponseDto<ProductDto>(product, "Product created successfully"));
        }

        [HttpPut("{id}")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto dto)
        {
            int userId = 1;
            await _service.UpdateAsync(id, dto, userId);
            return Ok(new ApiResponseDto<string>(null, "Product updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new ApiResponseDto<string>("Product deleted successfully"));
        }
    }
}