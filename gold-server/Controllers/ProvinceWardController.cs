using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs.common;
using gold_server.DTOs.Province;
using gold_server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace gold_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvinceWardController : ControllerBase
    {
        private readonly IProvinceWardImportService _importService;

        public ProvinceWardController(IProvinceWardImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponseDto<string>("Uploaded file is empty."));
            }

            try
            {
                await _importService.ImportFromJsonAsync(file.OpenReadStream());
                return Ok(new ApiResponseDto<string>(null, "Import completed successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto<string>($"Server error: {ex.Message}"));
            }
        }
        [HttpGet("import-default")]
        public async Task<IActionResult> ImportDefault()
        {
            try
            {
                await _importService.ImportFromJsonAsync(null);
                return Ok(new ApiResponseDto<string>(null, "Import Provinces and Wards completed successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto<string>($"Server error: {ex.Message}"));
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProvinces()
        {
            try
            {
                var provinces = await _importService.GetAllProvincesAsync();
                return Ok(new ApiResponseDto<List<ProvinceDto>>(provinces, "Provinces retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto<string>($"Server error: {ex.Message}"));
            }
        }

        [HttpGet("GetProvinceById/{id}")]
        public async Task<IActionResult> GetProvinceById(int id)
        {
            try
            {
                var province = await _importService.GetProvinceByIdAsync(id);
                return Ok(new ApiResponseDto<ProvinceDto>(province, $"Province with ID {id} retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto<string>($"Server error: {ex.Message}"));
            }
        }
    }
}



