using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using gold_server.Configs;
using gold_server.Constants;
using gold_server.DTOs;
using gold_server.DTOs.User;
using gold_server.Models;
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
            try
            {
                var user = await _context.USERs.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null || !VerifyPassword(request.Password, user.Password_Hash))
                    return null;

                var role = await _context.ROLEs
                    .Where(r => r.ID_Role == user.ID_Role)
                    .Select(r => new
                    {
                        r.Name,
                        Permissions = r.PERMISSIONs.Select(p => p.Name).ToList()
                    }).FirstOrDefaultAsync();

                if (role == null) return null;

                return GenerateJwtToken(user, role.Name, role.Permissions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                if (await _context.USERs.AnyAsync(u => u.Email == request.Email))
                    return false;

                var role = await _context.ROLEs.FirstOrDefaultAsync(r => r.ID_Role == AuthConstants.CustomerRoleId);
                if (role == null)
                    return false;
                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var vietnamNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

                var user = new USER
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    Phone = request.Phone,
                    Password_Hash = HashPassword(request.Password),
                    ID_Role = role.ID_Role,
                    CreateDate = vietnamNow
                };

                _context.USERs.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"DB update error: {dbEx.InnerException?.Message ?? dbEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.RefreshSecret);

                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var tokenType = principal.FindFirst(AuthConstants.TokenTypeClaim)?.Value;
                if (tokenType != AuthConstants.RefreshTokenType)
                    return null;

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (userId == 0)
                    return null;

                var user = await _context.USERs.FirstOrDefaultAsync(u => u.ID_User == userId);
                if (user == null)
                    return null;

                var role = await _context.ROLEs
                    .Where(r => r.ID_Role == user.ID_Role)
                    .Select(r => new
                    {
                        r.Name,
                        Permissions = r.PERMISSIONs.Select(p => p.Name).ToList()
                    }).FirstOrDefaultAsync();

                if (role == null)
                    return null;

                return GenerateJwtToken(user, role.Name, role.Permissions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RefreshTokenAsync: {ex.Message}");
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
            var now = DateTime.UtcNow;
            var accessExp = now.AddMinutes(AuthConstants.AccessTokenMinutes);
            var refreshExp = now.AddDays(AuthConstants.RefreshTokenDays);

            var accessClaims = BuildAccessClaims(user, roleName, permissions);
            var refreshClaims = BuildRefreshClaims(user);

            var accessToken = CreateJwt(accessClaims, accessExp, _jwtSettings.Secret, _jwtSettings.Issuer, _jwtSettings.Audience);
            var refreshToken = CreateJwt(refreshClaims, refreshExp, _jwtSettings.RefreshSecret, _jwtSettings.Issuer, _jwtSettings.Audience);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = accessExp, // UTC time
                Role = roleName,
                Permissions = permissions
            };
        }

        private static IEnumerable<Claim> BuildAccessClaims(USER user, string roleName, IEnumerable<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID_User.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(AuthConstants.RoleClaim, roleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            foreach (var p in permissions)
            {
                claims.Add(new Claim(AuthConstants.PermissionClaim, p));
            }

            return claims;
        }

        private static IEnumerable<Claim> BuildRefreshClaims(USER user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID_User.ToString()),
                new Claim(AuthConstants.TokenTypeClaim, AuthConstants.RefreshTokenType),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
        }

        private static string CreateJwt(IEnumerable<Claim> claims, DateTime expiresUtc, string secret, string issuer, string audience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresUtc,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> AssignPermissionsToRoleAsync(AssignPermissionRequestDto request)
        {
            try
            {
                var role = await _context.ROLEs
                    .Include(r => r.PERMISSIONs)
                    .FirstOrDefaultAsync(r => r.ID_Role == request.RoleId);

                if (role == null)
                    return false;

                var permissions = await _context.PERMISSIONs
                    .Where(p => request.PermissionIds.Contains(p.ID_Permission))
                    .ToListAsync();

                role.PERMISSIONs = permissions;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AssignPermissionsToRoleAsync: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> CreateRoleAsync(CreateRoleRequestDto request)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateRoleAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            try
            {
                return await _context.USERs
                    .Include(u => u.RoleNavigation)
                    .Select(u => new UserResponseDto
                    {
                        ID_User = u.ID_User,
                        FullName = u.FullName,
                        Email = u.Email,
                        Phone = u.Phone,
                        RoleName = u.RoleNavigation.Name,
                        CreateDate = u.CreateDate
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsersAsync: {ex.Message}");
                return new List<UserResponseDto>();
            }
        }
    }
}
