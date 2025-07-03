using gold_server.DTOs;
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
                    return Unauthorized("Invalid credentials");
                return Ok(result);
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
                    return BadRequest("Email already exists or registration failed.");
                return Ok("Registration successful.");
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
                    return NotFound("Role not found or failed to assign permissions.");
                return Ok("Permissions assigned successfully.");
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
                    return BadRequest("Role already exists or invalid input.");

                return Ok("Role created successfully.");
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
                return Ok(users);
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
                    return Unauthorized("Invalid or expired refresh token.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}