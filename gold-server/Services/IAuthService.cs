using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs;
using gold_server.DTOs.User;

namespace gold_server.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);
        Task<bool> RegisterAsync(RegisterRequestDto request);
        Task<bool> AssignPermissionsToRoleAsync(AssignPermissionRequestDto request);
        Task<bool> CreateRoleAsync(CreateRoleRequestDto request);
        Task<List<UserResponseDto>> GetAllUsersAsync();
    }
}