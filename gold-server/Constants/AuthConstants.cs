using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.Constants
{
    public class AuthConstants
    {
        public const int CustomerRoleId = 4;
        public const int AccessTokenMinutes = 5;
        public const int RefreshTokenDays = 7;
        public const string RoleClaim = "Role";
        public const string PermissionClaim = "Permission";
        public const string TokenTypeClaim = "TokenType";
        public const string RefreshTokenType = "RefreshToken";
    }
}