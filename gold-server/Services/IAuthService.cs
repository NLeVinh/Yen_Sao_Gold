using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs;

namespace gold_server.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}