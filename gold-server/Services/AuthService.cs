using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using gold_server.Configs;
using gold_server.DTOs;
using gold_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace gold_server.Services
{
    public class AuthService : IAuthService
    {
        private readonly GoldServerContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<USER> _userManager;
        private readonly SignInManager<USER> _signInManager;

        public AuthService(GoldServerContext context, JwtSettings jwtSettings, UserManager<USER> userManager,
                              SignInManager<USER> signInManager)
        {
            _context = context;
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}