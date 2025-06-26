using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs.User
{
    public class UserResponseDto
    {
        public string ID_User { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}