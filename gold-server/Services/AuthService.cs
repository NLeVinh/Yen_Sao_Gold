using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using gold_server.Configs;
using gold_server.DTOs;
using gold_server.DTOs.User;
using gold_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace gold_server.Services
{
    public class AuthService : IAuthService
    {
        private readonly GoldServerContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(GoldServerContext context, IOptions<JwtSettings> jwtOptions)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.USERs.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !VerifyPassword(request.Password, user.Password_Hash))
                return null;

            var role = await _context.ROLEs
                .Where(r => r.ID_Roles == user.ID_Role)
                .Select(r => new
                {
                    r.Name,
                    Permissions = r.ID_Permissions.Select(p => p.Name).ToList()
                }).FirstOrDefaultAsync();

            if (role == null) return null;

            return GenerateJwtToken(user, role.Name, role.Permissions);
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            if (await _context.USERs.AnyAsync(u => u.Email == request.Email))
                return false;

            var role = await _context.ROLEs.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (role == null)
                return false;

            var user = new USER
            {
                // ID_User = Guid.NewGuid().ToString(),
                Email = request.Email,
                FullName = request.FullName,
                Phone = request.Phone,
                Password_Hash = HashPassword(request.Password),
                ID_Role = role.ID_Roles,
                CreateDate = DateTime.UtcNow
            };

            _context.USERs.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // no extra leeway
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                    return null;

                var user = await _context.USERs.FirstOrDefaultAsync(u => u.ID_User == userId);
                if (user == null)
                    return null;

                var role = await _context.ROLEs
                    .Where(r => r.ID_Roles == user.ID_Role)
                    .Select(r => new
                    {
                        r.Name,
                        Permissions = r.ID_Permissions.Select(p => p.Name).ToList()
                    }).FirstOrDefaultAsync();

                if (role == null)
                    return null;

                return GenerateJwtToken(user, role.Name, role.Permissions);
            }
            catch
            {
                return null;
            }
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string input, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashed);
        }

        private LoginResponseDto GenerateJwtToken(USER user, string roleName, List<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID_User),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("Role", roleName)
            };

            foreach (var perm in permissions)
                claims.Add(new Claim("Permission", perm));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var accessTokenExpires = DateTime.UtcNow.AddMinutes(5);
            var accessToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: accessTokenExpires,
                signingCredentials: creds
            );

            // Refresh token (JWT) with longer expiry and fewer claims
            var refreshClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID_User),
                new Claim("TokenType", "RefreshToken")
            };
            var refreshTokenExpires = DateTime.UtcNow.AddDays(7);
            var refreshToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: refreshClaims,
                expires: refreshTokenExpires,
                signingCredentials: creds
            );

            return new LoginResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                ExpiresAt = accessTokenExpires,
                Role = roleName,
                Permissions = permissions
            };
        }

        public async Task<bool> AssignPermissionsToRoleAsync(AssignPermissionRequestDto request)
        {
            var role = await _context.ROLEs
                .Include(r => r.ID_Permissions)
                .FirstOrDefaultAsync(r => r.ID_Roles == request.RoleId);

            if (role == null)
                return false;

            var permissions = await _context.PERMISSIONs
                .Where(p => request.PermissionIds.Contains(p.ID_Permission))
                .ToListAsync();

            // Gán danh sách permission mới
            role.ID_Permissions = permissions;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CreateRoleAsync(CreateRoleRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return false;

            var exists = await _context.ROLEs.AnyAsync(r => r.Name == request.Name);
            if (exists)
                return false;

            var newRole = new ROLE
            {
                Name = request.Name,
                Description = request.Description
            };

            _context.ROLEs.Add(newRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            return await _context.USERs
                .Include(u => u.ID_RoleNavigation)
                .Select(u => new UserResponseDto
                {
                    ID_User = u.ID_User,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleName = u.ID_RoleNavigation.Name,
                    CreateDate = u.CreateDate
                })
                .ToListAsync();
        }
    }
}
