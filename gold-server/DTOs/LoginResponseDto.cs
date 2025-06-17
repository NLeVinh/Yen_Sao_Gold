using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}