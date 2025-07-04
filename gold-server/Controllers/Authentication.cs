using gold_server.DTOs;
using gold_server.DTOs.common;
using gold_server.DTOs.User;
using gold_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace gold_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                if (result == null)
                    return Unauthorized(new ApiResponseDto<string>("Invalid credentials"));
                return Ok(new ApiResponseDto<LoginResponseDto>(result, "Login successful."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var success = await _authService.RegisterAsync(request);
                if (!success)
                    return BadRequest(new ApiResponseDto<string>("Email already exists or registration failed."));
                return Ok(new ApiResponseDto<string>("Registration successful."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("assign-permissions-to-role")]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionRequestDto request)
        {
            try
            {
                var result = await _authService.AssignPermissionsToRoleAsync(request);
                if (!result)
                    return NotFound(new ApiResponseDto<string>("Role not found or failed to assign permissions."));
                return Ok(new ApiResponseDto<string>("Permissions assigned successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestDto request)
        {
            try
            {
                var success = await _authService.CreateRoleAsync(request);
                if (!success)
                    return BadRequest(new ApiResponseDto<string>("Role already exists or invalid input."));

                return Ok(new ApiResponseDto<string>("Role created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();
                return Ok(new ApiResponseDto<IEnumerable<UserResponseDto>>(users, "Users retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshToken);
                if (result == null)
                    return Unauthorized(new ApiResponseDto<string>("Invalid or expired refresh token."));
                return Ok(new ApiResponseDto<LoginResponseDto>(result, "Token refreshed successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}