using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gold_server.Services;
using gold_server.DTOs.common;
using gold_server.DTOs.Image;

namespace gold_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _service;

        public ImageController(IImageService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromQuery] string? folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = "Products";
            }

            var result = await _service.UploadImageAsync(file, folder);
            return Ok(new ApiResponseDto<ImageDto>(result, "Upload successful"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await _service.GetAllImagesAsync();
            return Ok(new ApiResponseDto<IEnumerable<ImageDto>>(images));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteImageAsync(id);
            return Ok(new ApiResponseDto<string>("Image deleted successfully"));
        }
    }
}