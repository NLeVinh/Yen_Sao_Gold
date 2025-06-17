using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.Configs
{
    public class JwtSettings
    {
        public required string Secret { get; set; }
        public int TokenLifetime { get; set; }
        public int RefreshTokenLifetime { get; set; }
    }
}